using Autofac;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QueueOverflow.Infrastructure;
using QueueOverflow.Infrastructure.Membership;
using QueueOverflow.Web.Areas.user.Models;
using static System.Formats.Asn1.AsnWriter;

namespace QueueOverflow.Web.Areas.user.Controllers
{
	[Area("user")]
    public class ReplyController : Controller
    {
		private UserManager<ApplicationUser> _userManager;
		private ILifetimeScope _scope;
		private ILogger<ReplyController> _logger;
		public ReplyController(UserManager<ApplicationUser> userManager
			, ILifetimeScope scope
			, ILogger<ReplyController> logger)
		{
			_userManager = userManager;
			_scope = scope;
			_logger = logger;
		}
		public IActionResult Index()
        {
            return View();
        }
		[Authorize(Policy = "QuestionReplyingPolicy")]
		[HttpPost,ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(ViewSpecificQuestionModel model)
		{
			//If error uncomment this to check
			//var errors = ModelState.Values.Where(m => m.Errors.Count > 0).ToList();
			if(ModelState.IsValid)
			{
				model.Resolve(_scope);
				try
				{
					Guid userId = GetCurrentUserId();
					await model.CreateReplyAsync();
					await model.UpdatePointOnReplyAsync();
					TempData["SuccessMessage"] = "Reply added";
					return RedirectToRoute(new
					{
						area = "user",
						controller = "Question",
						action = "View",
						id = model.ReplyToPost.QuestionId
					});
				}
				catch(Exception ex)
				{
					_logger.LogError("An error occurred of : " + ex);
					TempData["ErrorMessage"] = "An error occurred";
					return RedirectToRoute(new
					{
						area = "user",
						controller = "Question",
						action = "View",
						id = model.ReplyToPost.QuestionId
					});
				}
			}
			return RedirectToRoute(new 
			{
				area = "user",
				controller="Question",
				action= "Questions",

			});
		}
		public Guid GetCurrentUserId()
		{
			return Guid.Parse(_userManager.GetUserId(HttpContext.User));
		}
	}
}
