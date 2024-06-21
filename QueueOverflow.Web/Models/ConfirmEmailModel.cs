using Autofac;
using Microsoft.AspNetCore.Identity;
using QueueOverflow.Application.Features.Services;
using QueueOverflow.Application.Utilities;
using QueueOverflow.Infrastructure.Membership;
using static System.Formats.Asn1.AsnWriter;

namespace QueueOverflow.Web.Models
{
	public class ConfirmEmailModel
	{
		private UserManager<ApplicationUser> _userManager;
		private SignInManager<ApplicationUser> _signInManager;
		private ILifetimeScope _scope;
		public ConfirmEmailModel()
		{

		}
		public ConfirmEmailModel(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
        }
		public async Task ConfirmEmailAsync(Guid userId)
		{
			string uid = userId.ToString();
			var user  = await _userManager.FindByIdAsync(uid);
			if(user is {}) 
			{
				user.EmailConfirmed = true;
				await _userManager.UpdateAsync(user);
				await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Email Confirmed", "true"));
				await _signInManager.RefreshSignInAsync(user);
			}
		}
		internal void Resolve(ILifetimeScope scope)
		{
			_scope = scope;
			_userManager = _scope.Resolve<UserManager<ApplicationUser>>();
			_signInManager = _scope.Resolve<SignInManager<ApplicationUser>>();
		}
	}
}
