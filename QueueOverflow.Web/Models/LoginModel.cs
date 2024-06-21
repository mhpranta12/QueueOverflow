using Autofac;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QueueOverflow.Infrastructure.Membership;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace QueueOverflow.Web.Models
{
    public class LoginModel
    {
        private ILifetimeScope _scope;
        private SignInManager<ApplicationUser> _signInManager;
        private ILogger<LoginModel> _logger;
        public LoginModel() { }
        public LoginModel(ILifetimeScope scope,SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger)
        {
            _scope = scope;
            _signInManager = signInManager;
            _logger = logger;
        }
        public string? ReturnUrl { get; set; }
        [TempData]
        public string? ErrorMessage { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _signInManager = _scope.Resolve<SignInManager<ApplicationUser>>();
        }
        public async Task<string> LoginAsync(string returnUrl = null)
        {
            ReturnUrl??=returnUrl;
            var result = await _signInManager.PasswordSignInAsync(Email, Password, RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return ReturnUrl;
            }
            if (result.IsLockedOut)
            {
                ReturnUrl = "./Lockout";
            }
            return ReturnUrl;
        }
    }
}
