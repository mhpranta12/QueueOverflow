using QueueOverflow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueOverflow.Application.Features.Services
{
    public interface IUserInfoManagementService 
    {
        Task CreateUserInfoAsync(Guid Id,
            string? name = "", string? designation = "",
            string? about = "",
            string? portfolioLink = "",
            string? address = "", string? gitHubLink = "", string? badge = "");
        Task<UserInfo> GetCurrentUserInfo(Guid Id);
        Task EditUserInfoAsync(Guid Id,
			string? name = null, string? designation = null,
			string? about = null,
			string? portfolioLink = null,
			string? address = null, string? gitHubLink = null);
        Task<UserInfo> GetUserAsync(Guid Id);
        Task<List<Question>> GetQuestionByUserIdAsync(Guid userId);
        Task AddAnswerGivenAsync(Guid Id);
        Task AddTotalQuestionAsync(Guid Id);
        Task UpdateUsersReputationsAsync(Guid userId, uint points);
        Task UpdateBadges(Guid userId);
        Task<uint> CheckUsersPointAsync(Guid userId);

    }
}
