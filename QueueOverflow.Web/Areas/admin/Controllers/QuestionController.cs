using Autofac;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QueueOverflow.Infrastructure.Membership;
using QueueOverflow.Web.Areas.admin.Models;

namespace QueueOverflow.Web.Areas.admin.Controllers
{
	[Area("admin")]
	public class QuestionController : Controller
	{
		private UserManager<ApplicationUser> _userManager;
		private ILifetimeScope _scope;
		private ILogger<QuestionController> _logger;
		public QuestionController(UserManager<ApplicationUser> userManager
			, ILifetimeScope scope
			, ILogger<QuestionController> logger)
		{
			_userManager = userManager;
			_scope = scope;
			_logger = logger;
		}
		public IActionResult Index()
		{
			return View();
		}
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Questions()
		{
			var model = _scope.Resolve<ViewAllQuestionModelAdmin>();
			try
			{
				await model.GetAllQuestionsAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError("An error occured in questions regarding : " + ex);
				TempData["ErrorMessage"] = "Error to load";
			}
			return View(model);
		}
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(Guid Id)
		{
			var model = _scope.Resolve<DeleteQuestionModelAdmin>();
			try
			{
				await model.RemoveQuestionAsync(Id);
				TempData["SuccessMessage"] = "Question Deleted";
			}
			catch (Exception ex)
			{
				_logger.LogError("An error occured in questions regarding : " + ex);
				ViewBag.ErrorMessage = "An error occured on deleting Question";
				return RedirectToRoute(new
				{
					area = "admin",
					controller = "Question",
					action = "Questions"
				});
			}
			return RedirectToRoute(new
			{
				area = "admin",
				controller = "Question",
				action = "Questions"
			});
		}
	}
}
