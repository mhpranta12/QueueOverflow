using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using QueueOverflow.Application.Features.Services;
using QueueOverflow.Application.Utilities;
using QueueOverflow.Domain.Entities;
using QueueOverflow.Infrastructure.Membership;
using static System.Formats.Asn1.AsnWriter;

namespace QueueOverflow.Web.Areas.user.Models
{
    public class ViewProfileModel
    {
        private SignInManager<ApplicationUser> _signInManager;
        private IUserInfoManagementService _userInfoManagementService;
        private ILifetimeScope _scope;
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public uint Reputations { get; set; }
        public string About { get; set; }
        public DateTime JoinedDate { get; set; }
        public int? YearsOfJoining { get; set; }
        public int? MonthsOfJoining { get; set; }
        public string PortfolioLink { get; set; }
        public string Address { get; set; }
        public string GithubLink { get; set; }
        public string ProfilePictureLink { get; set; }
        public uint AnswersGiven { get; set; }
        public uint TotalQuestions { get; set; }
        public string Badge { get; set; }
        public List<Question>? UsersQuestions { get; set; }
        public string? UpdateMessage { get; set; }
        public ViewProfileModel()
        {

        }
        public ViewProfileModel(SignInManager<ApplicationUser> signInManager,
            IUserInfoManagementService userInfoManagementService,
			ILifetimeScope scope)
        {
            _signInManager = signInManager;
            _userInfoManagementService = userInfoManagementService;
            _scope = scope;
            //YearsOfJoining = DateTime.Now.Year - JoinedDate.Year;
        }
        public async Task LoadProfileAsync(Guid userId)
        {
            var userInfo = await _userInfoManagementService.GetCurrentUserInfo(userId);
            if(userInfo is not null)
            {
                Id = userId;
                Name = userInfo.Name;
                Designation = userInfo.Designation;
                Reputations = userInfo.Reputations;
                About = userInfo.About;
                JoinedDate = userInfo.JoinedDate;
                PortfolioLink = userInfo.PortfolioLink;
                Address = userInfo.Address;
                GithubLink = userInfo.GithubLink;
                TotalQuestions = userInfo.TotalQuestions;
                AnswersGiven = userInfo.AnswersGiven;
                Badge = userInfo.Badge;
            }
            YearsOfJoining =  DateTime.Now.Year - JoinedDate.Year;
            MonthsOfJoining = DateTime.Now.Month - JoinedDate.Month;

		}
        public async Task LoadQuestionsAsync()
        {
            UsersQuestions = await _userInfoManagementService.GetQuestionByUserIdAsync(Id);
        }
        internal void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _signInManager = _scope.Resolve<SignInManager<ApplicationUser>>();
            _userInfoManagementService = _scope.Resolve<IUserInfoManagementService>();
        }
    }
}
