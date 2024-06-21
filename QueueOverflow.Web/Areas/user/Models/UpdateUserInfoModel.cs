using Autofac;
using AutoMapper;
using QueueOverflow.Application.Features.Services;
using QueueOverflow.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace QueueOverflow.Web.Areas.user.Models
{
    public class UpdateUserInfoModel
    {
        private IUserInfoManagementService _userInfoManagementService;
        private IMapper _mapper;
        private ILifetimeScope _scope;

        [Required]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }

        public UpdateUserInfoModel() { }
        public UpdateUserInfoModel(IMapper mapper,
            IUserInfoManagementService userInfoManagementService,
            ILifetimeScope scope)
        {
            _mapper = mapper;
            _userInfoManagementService = userInfoManagementService;
            _scope = scope;
        }
        //public async Task LoadProfileAsync(Guid Id)
        //{
        //    UserInfo userInfo = await _userInfoManagementService.GetUserAsync(Id);
        //    if (userInfo is not null)
        //    {
        //        _mapper.Map(userInfo, this);
        //        var curent = this;
        //    }
        //}
        internal void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _userInfoManagementService = _scope.Resolve<IUserInfoManagementService>();
            _mapper = _scope.Resolve<IMapper>();
        }
        public async Task EditProfileAsync()
        {
            //await _userInfoManagementService.EditUserInfoAsync(Id, Name, Designation, About, PortfolioLink, Address, GithubLink);
        }
    }
}
