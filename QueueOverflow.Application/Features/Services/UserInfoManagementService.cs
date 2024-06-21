using QueueOverflow.Domain.Entities;
using QueueOverflow.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QueueOverflow.Application.Features.Services
{
    public class UserInfoManagementService: IUserInfoManagementService
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public UserInfoManagementService(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateUserInfoAsync(Guid Id,
            string? name="", string? designation="", 
            string? about="",
            string? portfolioLink="",
            string? address="", string? gitHubLink="",string? badge="")
        {
            UserInfo userInfo = new UserInfo()
            {
                Id = Id,
                Name = name,
                Designation = designation,
                Reputations = 21,
                About = about,
                JoinedDate = DateTime.Now.Date,
                Address = address,
                PortfolioLink = portfolioLink,
                GithubLink = gitHubLink,
                AnswersGiven =0,
                TotalQuestions = 0,
                Badge = "n/a" 
            };
            await _unitOfWork.UserInfoRepository.AddAsync(userInfo);
            await _unitOfWork.SaveAsync();
        }

		public async Task EditUserInfoAsync(Guid Id, string? name = null, string? designation = null, string? about = null, string? portfolioLink = null, string? address = null, string? gitHubLink = null)
		{
            UserInfo userInfo = await GetUserAsync(Id);
            //If user name is duplicate then throw custom exception
			bool isDuplicatName= await _unitOfWork.UserInfoRepository.
				IsUserNameDuplicateAsync(name,Id);

			if (isDuplicatName)
				throw new DuplicateUserNameException();

			//If found, then setting new values except Id
			if (userInfo is { })
            {
                userInfo.Name = name;
                userInfo.Designation = designation;
                userInfo.About = about;
                userInfo.PortfolioLink = portfolioLink;
                userInfo.GithubLink = gitHubLink;
                userInfo.Address = address;
			}
			await _unitOfWork.UserInfoRepository.EditAsync(userInfo);
			await _unitOfWork.SaveAsync();
		}
		public async Task AddAnswerGivenAsync(Guid Id)
        {
			UserInfo userInfo = await GetUserAsync(Id);
			//If found, then setting new values except Id
			if (userInfo is { })
			{
                userInfo.AnswersGiven += 1;
			}
			await _unitOfWork.UserInfoRepository.EditAsync(userInfo);
			await _unitOfWork.SaveAsync();
		}
        public async Task AddTotalQuestionAsync(Guid Id)
        {
			UserInfo userInfo = await GetUserAsync(Id);
			//If found, then setting new values except Id
			if (userInfo is { })
			{
                userInfo.TotalQuestions += 1;
			}
			await _unitOfWork.UserInfoRepository.EditAsync(userInfo);
			await _unitOfWork.SaveAsync();
		}
		public async Task<UserInfo> GetCurrentUserInfo(Guid Id)
        {
            UserInfo userInfo =  await _unitOfWork.UserInfoRepository.GetByIdAsync(Id);
            return userInfo;
        }
        //User Points Related
		public async Task<List<Question>> GetQuestionByUserIdAsync(Guid userId)
		{
           return await _unitOfWork.UserInfoRepository.GetQuestionByUserIdAsync(userId);
		}

		public async Task<UserInfo> GetUserAsync(Guid Id)
        {
            return await _unitOfWork.UserInfoRepository.GetByIdAsync(Id);
        }
        public async Task<uint> CheckUsersPointAsync(Guid userId)
        {
            var userPoints = await GetUserAsync(userId);
            if (userPoints is not null)
            {
                return userPoints.Reputations;
            }
            return 0;
        }
        public async Task UpdateUsersReputationsAsync(Guid userId, uint points)
        {
            UserInfo userInfo = await _unitOfWork.UserInfoRepository.GetByIdAsync(userId);
            if (userInfo is not null)
            {
                userInfo.Reputations += points;
                await _unitOfWork.UserInfoRepository.EditAsync(userInfo);
                await _unitOfWork.SaveAsync();
                await UpdateBadges(userId);
            }
        }
        public async Task UpdateBadges(Guid userId)
        {
            UserInfo userInfo = await _unitOfWork.UserInfoRepository.GetByIdAsync(userId);
            if (userInfo is not null)
            {
                if (userInfo is not null)
                {
                    if (userInfo.Reputations > 60)
                    {
                        userInfo.Badge = "Bronze";
                    }
                    if (userInfo.Reputations > 80)
                    {
                        userInfo.Badge = "Silver";
                    }
                    if (userInfo.Reputations > 100)
                    {
                        userInfo.Badge = "Gold";
                    }
                }
                await _unitOfWork.UserInfoRepository.EditAsync(userInfo);
                await _unitOfWork.SaveAsync();
            }
        }
    }
}
