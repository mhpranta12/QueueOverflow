using Autofac;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QueueOverflow.Infrastructure.Membership;
using QueueOverflow.Web.Areas.user.Models;

namespace QueueOverflow.Web.Areas.user.Controllers
{
	[Area("user")]
	public class CommentController : Controller
	{
		private UserManager<ApplicationUser> _userManager;
		private ILifetimeScope _scope;
		private ILogger<ReplyController> _logger;
		public CommentController(UserManager<ApplicationUser> userManager
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
		[Authorize(Policy = "QuestionCommentingPolicy")]
		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(ViewSpecificQuestionModel model)
		{
			Guid userId = GetCurrentUserId();
			var errors = ModelState.Values.Where(m => m.Errors.Count > 0).ToList();
			if (ModelState.IsValid)
			{
				model.Resolve(_scope);
				bool canComment = await model.CanCommentAsync();
				if(canComment)
				{
					try
					{
						await model.CreateCommentAsync();
						await model.UpdatePointOnCommentAsync();
						TempData["SuccessMessage"] = "Comment Added";
						return RedirectToRoute(new
						{
							area = "user",
							controller = "Question",
							action = "View",
							id = model.CommentToPost.QuestionId
						});
					}
					catch (Exception ex)
					{
						_logger.LogError("An error occured in comments regarding : " + ex);
						TempData["ErrorMessage"] = "An internal error occured";
					}
				}
				else
				{
					return RedirectToAction("SorryMsgPoint");
				}
			}
			return RedirectToRoute(new
			{
				area = "user",
				controller = "Question",
				action = "Questions",
			});
		}
		[Authorize(Policy = "QuestionPostingPolicy")]
		public async Task<IActionResult> SorryMsgPoint()
		{
			return View();
		}
		public Guid GetCurrentUserId()
		{
			return Guid.Parse(_userManager.GetUserId(HttpContext.User));
		}

	}
}
