using Autofac;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QueueOverflow.Infrastructure.Membership;
using QueueOverflow.Web.Areas.user.Models;

namespace QueueOverflow.Web.Areas.user.Controllers
{
	[Area("user")]
	public class TagController : Controller
	{
		private ILifetimeScope _scope;
		private ILogger<TagController> _logger;
		public TagController(ILifetimeScope scope
			, ILogger<TagController> logger)
		{
			_scope = scope;
			_logger = logger;
		}
		public IActionResult Index()
		{
			return View();
		}
		public async Task<IActionResult> View(Guid Id)
		{
			var model = _scope.Resolve<ViewAllQuestionModel>();
			try
			{
				await model.GetQuestionsByTagIdAsync(Id);
			}
			catch(Exception ex) 
			{
				_logger.LogError("error occured in tag controller of : " + ex);
				TempData["ErrorMessage"] = "An internal error occured";
			}
			return View(model);
		}
	}
}
