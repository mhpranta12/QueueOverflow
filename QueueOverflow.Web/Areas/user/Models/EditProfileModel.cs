using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using QueueOverflow.Application.Features.Services;
using QueueOverflow.Domain.Entities;
using QueueOverflow.Infrastructure.Membership;
using System.ComponentModel.DataAnnotations;

namespace QueueOverflow.Web.Areas.user.Models
{
	public class EditProfileModel
	{
		private IUserInfoManagementService _userInfoManagementService;
		private IMapper _mapper;
		private ILifetimeScope _scope;

		public Guid Id { get; set; }
		[Required(ErrorMessage = "User Name is required")]
		public string Name { get; set; }
		public string? Designation { get; set; }
		public uint Reputations { get; set; }
        //[Required(ErrorMessage = "About section is required"),StringLength(50,MinimumLength =5,ErrorMessage ="Must have to be al least 5 words")]
        public string? About { get; set; }
		public string? PortfolioLink { get; set; }
		public string? Address { get; set; }
		public string? GithubLink { get; set; }
		public string? ProfilePictureLink { get; set; }
		public EditProfileModel() { }
		public EditProfileModel(IMapper mapper,
			IUserInfoManagementService userInfoManagementService,
			ILifetimeScope scope)
		{
			_mapper = mapper;
			_userInfoManagementService = userInfoManagementService;
			_scope = scope;
		}
		public async Task LoadProfileAsync(Guid Id)
		{
			UserInfo userInfo = await _userInfoManagementService.GetUserAsync(Id);
			if(userInfo is not null)
			{
				_mapper.Map(userInfo,this);
				var curent = this;
			}
		}
		internal void Resolve(ILifetimeScope scope)
		{
			_scope = scope;
			_userInfoManagementService = _scope.Resolve<IUserInfoManagementService>();
			_mapper = _scope.Resolve<IMapper>();
		}
		public async Task EditProfileAsync()
		{
			await _userInfoManagementService.EditUserInfoAsync(Id, Name, Designation, About, PortfolioLink, Address, GithubLink);
		}
	}
}
