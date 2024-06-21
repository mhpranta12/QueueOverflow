using Autofac;
using QueueOverflow.Application.Features.Services;
using QueueOverflow.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace QueueOverflow.Web.Areas.user.Models
{
    public class CreateQuestionModel
    {
        private IQuestionManagementService _questionManagementService;
        private IUserInfoManagementService _userInfoManagementService;
        private ILifetimeScope _scope;

		[Required(ErrorMessage = "Title is required")]
		public string Title { get; set; }
		public string? UserName { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [StringLength(200,MinimumLength =10,ErrorMessage ="Must need 10 words")]
        public string Description { get; set; }
        public uint Like { get; set; }
        public uint Dislike { get; set; }
        [Required(ErrorMessage = "Tags are required")]
        [StringLength(30,ErrorMessage ="Max 30 and Min 2 char",MinimumLength =2)]
		public string Tags { get; set; }
		public Guid UserId { get; set; }
        public CreateQuestionModel() { }
        public CreateQuestionModel(IQuestionManagementService questionManagementService,
			IUserInfoManagementService userInfoManagementService)
        {
            _questionManagementService = questionManagementService;
            _userInfoManagementService = userInfoManagementService;
        }
        internal void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _questionManagementService = _scope.Resolve<IQuestionManagementService>();
            _userInfoManagementService = _scope.Resolve<IUserInfoManagementService>();
        }
        public async Task AttachUserIdAndNameAsync(Guid Id)
        {
            UserId = Id;
            var user = await _userInfoManagementService.GetUserAsync(UserId);
            if(user is { })
            {
                UserName = user.Name;
            }
        }
		public async Task<bool> CanPostQuestionAsync()
		{
			uint points = await _userInfoManagementService.CheckUsersPointAsync(UserId);
			return points > 20;
		}
		public async Task CreateQuestionAsync()
        {
            await _questionManagementService.CreateQuestionAsync(Title,Description,Tags,UserName,UserId);
            await _userInfoManagementService.AddTotalQuestionAsync(UserId);
        }
    }
}
