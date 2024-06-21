using Autofac;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QueueOverflow.Infrastructure.Membership;
using QueueOverflow.Web.Areas.user.Models;
using QueueOverflow.Infrastructure;
using QueueOverflow.Web.Models;
using static System.Formats.Asn1.AsnWriter;
using Microsoft.AspNetCore.Authorization;

namespace QueueOverflow.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILifetimeScope _scope;
        private readonly ILogger<AccountController> _logger;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        public AccountController(ILifetimeScope scope,
            IConfiguration config,
            ILogger<AccountController> logger,
            RoleManager<ApplicationRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager
            )
        {
            _scope = scope;
            _logger = logger;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = config;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            var model = _scope.Resolve<LoginModel>();
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                model.Resolve(_scope);
                string redirectLocation = "/";
				try
                {
					redirectLocation = await model.LoginAsync(Url.Content("~/"));
				}
				catch (Exception ex)
                {
					TempData.Put("ResponseMessge", new ResponseModel()
					{
						Message = "Credentials Error",
						Type = ResponseTypes.Danger,
					});
				}
				return Redirect(redirectLocation);
            }
            return View(model);
        }
        public IActionResult Register()
        {
            var model = _scope.Resolve<RegistrationModel>();
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationModel model)
        {
            //Uncmnt this to check error in depth
            //var defatcts = ModelState.Values.Where(x => x.Errors.Count > 0).ToList();

            if (ModelState.IsValid)
            {
                model.Resolve(_scope);
                var response =  await model.RegisterAsync(Url.Content("~/"));
                if (response.errors is not null)
                {
                    foreach (var error in response.errors)
                    {
                        ModelState.AddModelError(String.Empty, error.Description);
                    }
                }
                else
                    return Redirect(response.redirectLocation);
            }
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> LogoutAsync(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            if (returnUrl is not null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [Authorize]
        public async Task<IActionResult> ConfirmEmail(Guid Id)
        {
            var model = _scope.Resolve<ConfirmEmailModel>();
            await model.ConfirmEmailAsync(Id);
			return RedirectToRoute(new
			{
				area = "user",
				controller = "Question",
				action = "Questions",
			});
        }
		public async Task<IActionResult> AccessDenied()
        {
            return View();
        }
	}
}
