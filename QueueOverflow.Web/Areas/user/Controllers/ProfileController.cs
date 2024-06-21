using Autofac;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QueueOverflow.Domain.Exceptions;
using QueueOverflow.Infrastructure.Membership;
using QueueOverflow.Web.Areas.user.Models;
using System.Security.Claims;

namespace QueueOverflow.Web.Areas.user.Controllers
{
    [Area("user")]
    public class ProfileController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private ILifetimeScope _scope;
        private ILogger<ProfileController> _logger;
        public ProfileController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILifetimeScope scope,
            ILogger<ProfileController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _scope = scope;
            _logger = logger;
        }
        [Authorize(Policy = "ProfileViewingPolicy")]
        public async Task<IActionResult> View()
        {
			var model = _scope.Resolve<ViewProfileModel>();
			try
			{
				Guid Id = GetCurrentUserId();
				model.Resolve(_scope);
				await model.LoadProfileAsync(Id);
				await model.LoadQuestionsAsync();
			}
			catch (Exception ex)
            {
				_logger.LogError("An error occured in profile regarding : " + ex);
				TempData["ErrorMessage"] = "Error to load";
			}
            return View(model);
        }
        public async Task<IActionResult> ViewProfile(Guid Id)
        {
            var model = _scope.Resolve<ViewProfileModel>();
			try
			{
				model.Resolve(_scope);
				await model.LoadProfileAsync(Id);
				await model.LoadQuestionsAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError("An error occured in loading profile regarding : " + ex);
				TempData["ErrorMessage"] = "Error to load";
			}
			return View(model);
        }
        [Authorize(Policy = "ProfileEditingPolicy")]
        public async Task<IActionResult> Edit()
        {
                var model = _scope.Resolve<EditProfileModel>();
				try
				{
					Guid Id = GetCurrentUserId();
					await model.LoadProfileAsync(Id);
				}
				catch (Exception ex)
				{
					_logger.LogError("An error occured in editing profile regarding : " + ex);
					TempData["ErrorMessage"] = "Error to load";
				}
                return View(model);
        }
		[Authorize(Policy = "ProfileEditingPolicy")]
		[HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProfileModel model)
        {
            var errors = ModelState.Values.Where(x => x.Errors.Count > 0).ToList();
            if(ModelState.IsValid)
            {
                try
                {
                    try
                    {
						model.Resolve(_scope);
						await model.EditProfileAsync();
						TempData["SuccessMessage"] = "Profile Updated";
						return RedirectToRoute(
							new
							{
								area = "user",
								controller = "Profile",
								action = "View"
							});
					}
                    catch (DuplicateUserNameException dex)
                    {
						_logger.LogError("An error ocuured of  : " + dex);
						ViewBag.ErrorMessage = "This User Name is already taken. Please select any other name";
						return View(model);
					}
					
				}
                catch(Exception ex)
                {
                    _logger.LogError("An error ocuured of  : "+ex);
					ViewBag.ErrorMessage = "An internal error ocuured";
					return View(model);
				}
			}
            return View(model);
        }
		public async Task<IActionResult> PointPolicy()
		{
			return await View();
		}
        [Authorize(Policy = "ProfileViewingPolicy")]
        public Guid GetCurrentUserId()
        {
            return Guid.Parse(_userManager.GetUserId(HttpContext.User));
        }
    }
}
