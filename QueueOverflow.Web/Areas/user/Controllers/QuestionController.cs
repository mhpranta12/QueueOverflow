using Autofac;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QueueOverflow.Infrastructure.Membership;
using QueueOverflow.Infrastructure;
using QueueOverflow.Web.Areas.user.Models;
using QueueOverflow.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Org.BouncyCastle.Utilities.Encoders;

namespace QueueOverflow.Web.Areas.user.Controllers
{
	[Area("user")]

	public class QuestionController : Controller
	{
		private UserManager<ApplicationUser> _userManager;
		private SignInManager<ApplicationUser> _signInManager;
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
		public async Task<IActionResult> Questions()
		{
			var model = _scope.Resolve<ViewAllQuestionModel>();
			try
			{
				await model.GetAllQuestionsAsync();
			}
			catch(Exception ex)
			{
				_logger.LogError("An error occured in questions regarding : " + ex);
				TempData["ErrorMessage"] = "Error to load";
			}
			return View(model);
		}
		[Authorize(Policy = "QuestionViewingPolicy")]
		public async Task<IActionResult> View(Guid Id)
		{
			try
			{
				Guid userId = GetCurrentUserId();
				var model = _scope.Resolve<ViewSpecificQuestionModel>();
				model.GetQuestionById(Id);
				await model.LoadRepliesAsync(Id);
				await model.LoadCommentsAsync(Id);
				await model.AttachUserIdAndNameAsync(userId);
				return View(model);

			}
			catch (Exception ex)
			{
				_logger.LogError("An error occured in questions regarding : " + ex);
				TempData["ErrorMessage"] = "Error to load";
			}
			return View();
		}
		[Authorize(Policy = "QuestionPostingPolicy")]
		public async Task<IActionResult> Create()
		{
			Guid userId = GetCurrentUserId();
			var model = _scope.Resolve<CreateQuestionModel>();
			await model.AttachUserIdAndNameAsync(userId);
			bool canPost = await model.CanPostQuestionAsync();
			if(canPost)
			{
				return View(model);
			}
			return RedirectToRoute(new
			{
				area = "user",
				controller = "Comment",
				action = "SorryMsgPoint",
			});
		}
		[Authorize(Policy = "QuestionPostingPolicy")]
		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateQuestionModel model)
		{
			if (ModelState.IsValid)
			{
                model.Resolve(_scope);
                bool canPost = await model.CanPostQuestionAsync();
				if (canPost)
				{
					try
					{
						try
						{

                            await model.CreateQuestionAsync();
                            TempData["SuccessMessage"] = "Question Added";
                            return RedirectToAction("Questions");
                        }
						catch(DuplicateQuestionTitleException dex)
						{
                            _logger.LogError("An error occured in questions regarding : " + dex);
                            TempData["ErrorMessage"] = "There is already a question with this title, Please choose another one or view existing questions";
                            return View();
                        }
						catch(TagExceedsException tex)
						{
							_logger.LogError("An error occured in questions regarding : " + tex);
                            TempData["ErrorMessage"] = "Tag Must be maximum 3";
							return View();
						}

                    }
					catch (Exception ex)
					{
						_logger.LogError("An error occured in questions regarding : " + ex);
                        TempData["ErrorMessage"] = "An internal error occured";
						return View();
					}
				}
				return RedirectToRoute(new
				{
					area = "user",
					controller = "Comment",
					action = "SorryMsgPoint",
				});
			}
            ViewBag.ErrorMessage = "Validation Erorr";
            return RedirectToAction("Questions");
		}
		[HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> Search(ViewAllQuestionModel model)
		{
			if (model.SearchTitle is not null)
			{
				model.Resolve(_scope);
				await model.SearchQuestionsAsync();
			}
			else
			{
				return RedirectToAction("Questions");
			}
			return View(model);
		}
		[Authorize(Policy = "QuestionVotingPolicy")]
		public async Task<IActionResult> Like(Guid Id)
		{
			var model = _scope.Resolve<VoteQuestionModel>();
			await model.LikeQuestionAsync(Id);
			TempData["SuccessMessage"] = "Liked";
			return RedirectToRoute(new
			{
				area = "user",
				controller = "Question",
				action = "View",
				Id = Id,
			});
		}
        [Authorize(Policy = "QuestionVotingPolicy")]
		public async Task<IActionResult> UnLike(Guid Id)
		{
			var model = _scope.Resolve<VoteQuestionModel>();
			await model.UnLikeQuestionAsync(Id);
			TempData["ErrorMessage"] = "Unliked";
			return RedirectToRoute(new
			{
				area = "user",
				controller = "Question",
				action = "View",
				Id = Id,
			});
		}
		[Authorize(Policy = "QuestionVotingPolicy")]
		public async Task<IActionResult> Delete(Guid Id)
		{
			var model = _scope.Resolve<DeleteQuestionModel>();
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
					area = "user",
					controller = "Profile",
					action = "View"
				});
			}
			return RedirectToRoute(new
			{
				area = "user",
				controller = "Profile",
				action = "View"
			});
		}
		public Guid GetCurrentUserId()
		{
			return Guid.Parse(_userManager.GetUserId(HttpContext.User));
		}
	}
}
