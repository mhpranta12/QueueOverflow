using Autofac;
using MailKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QueueOverflow.Infrastructure.Membership;
using QueueOverflow.Web.Areas.user.Models;
using QueueOverflow.Web.Models;
using System.Diagnostics;
using static System.Formats.Asn1.AsnWriter;

namespace QueueOverflow.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILifetimeScope _scope;
		public HomeController(UserManager<ApplicationUser> userManager,
            ILogger<HomeController> logger,
            ILifetimeScope scope)
        {
            _userManager = userManager;
            _logger = logger;
            _scope = scope;
        }

        public async Task<IActionResult> Index()
        {
            if(_userManager.GetUserId(HttpContext.User) is not null)
            {
                if(HttpContext.User.IsInRole("Admin"))
                {
					return Redirect("/admin/Question/Questions");
				}
				else
                {
					return Redirect("/user/Question/Questions");
				}
			}
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Search(ViewQuestionsModel model)
        {
            if(model.SearchTitle is not null)
            {
				model.Resolve(_scope);
				await model.SearchQuestionsAsync();
			}
            else
            {
                return RedirectToAction("Index");
			}
            return View(model);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new Models.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
