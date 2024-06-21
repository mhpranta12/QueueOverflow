using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;
using System.Text.Encodings.Web;
using System.Text;
using QueueOverflow.Infrastructure.Membership;
using QueueOverflow.Application.Utilities;
using Autofac;
using QueueOverflow.Application.Features.Services;
using QueueOverflow.Domain.Entities;
using Amazon.S3;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using System.Text.Json;
using QueueOverflow.Infrastructure;
using Amazon.Runtime;

namespace QueueOverflow.Web.Models
{
    public class RegistrationModel
    {
        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;
        private IUserStore<ApplicationUser> _userStore;
        private IUserEmailStore<ApplicationUser> _emailStore;
        private ILogger<RegistrationModel> _logger;
        private IUserInfoManagementService _userInfoManagementService;
		private IEmailService _emailService;
        private ILifetimeScope _scope;
        public RegistrationModel() { }
        public RegistrationModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            IUserInfoManagementService userInfoManagementService,
            ILogger<RegistrationModel> logger,
            IEmailService emailService)
        {
            _userManager = userManager;
            _userStore = userStore;
            _signInManager = signInManager;
            _userInfoManagementService = userInfoManagementService;
            _logger = logger;
            _emailService = emailService;
        }

        [BindProperty]
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "FirstName")]
        public string? FirstName { get; set; }
        //[Required]
        [Display(Name = "LastName")]
        public string? LastName { get; set; }
        public string? ReturnUrl { get; set; }
        public async Task<(IEnumerable<IdentityError>? errors, string redirectLocation)> RegisterAsync(string urlPrefix)
        {
            ReturnUrl ??= urlPrefix;
            var user = new ApplicationUser() {Id=Guid.NewGuid(), UserName = Email, Email = Email, FirstName = "", LastName = "" };
            var result = await _userManager.CreateAsync(user, Password);

            if (result.Succeeded)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                //Assigning Claims
                await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("View Profile","true"));
                await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Edit Profile","true"));
                await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Post Question","true"));
                await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Search Question","true"));
                await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("View Questions","true"));
                await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Reply On Question","true"));
                await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Comment On Question","true"));
                await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Vote Question","true"));
                //Assigning Claims

                //Simultaneously adding in userinfo table 
                await _userInfoManagementService.CreateUserInfoAsync(user.Id,user.Email);
                //Simultaneously adding in userpoint table 

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var confirmationUrl = $"https://localhost:7037/Account/ConfirmEmail/{user.Id}";

                _emailService.SendSingleEmail(FirstName + LastName, Email, "Confirm your email",
                   $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(confirmationUrl)}'>clicking here</a>");

                //SQS part 
                //Sending to the Queue
                EmailInfo emailInfo = new EmailInfo() {Id = user.Id,EmailOfUser = Email };
                string accessKeyId = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
				string secretAccessKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
				var credentials = new BasicAWSCredentials(accessKeyId,secretAccessKey);
                var sqsClient = new AmazonSQSClient(credentials,RegionEndpoint.USEast1);
                SqsPublisher publisher = new SqsPublisher(sqsClient);
                await publisher.PublishAsync("pranta_sqs", emailInfo);

                await _signInManager.RefreshSignInAsync(user);
                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                {
                    string confirmationPageLink = $"RegisterConfirmation?email={Email}&returnUrl={ReturnUrl}";
                    return (null, confirmationPageLink);
                }
                else
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return (null, ReturnUrl);
                }
            }
            else
            {
                return (result.Errors, null);
            }
        }
        internal void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _userManager = _scope.Resolve<UserManager<ApplicationUser>>();
            _signInManager = _scope.Resolve<SignInManager<ApplicationUser>>();
            _userInfoManagementService = _scope.Resolve<IUserInfoManagementService>();
            _emailService = _scope.Resolve<IEmailService>();
        }
	}
}
