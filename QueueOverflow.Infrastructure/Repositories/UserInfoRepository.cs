using Microsoft.EntityFrameworkCore;
using QueueOverflow.Domain.Entities;
using QueueOverflow.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueOverflow.Infrastructure.Repositories
{
    public class UserInfoRepository :Repository<UserInfo, Guid>, IUserInfoRepository
    {
		private IApplicationDbContext _applicationDbContext;
		public UserInfoRepository(IApplicationDbContext dbContext) : base((DbContext)dbContext)
        {
			_applicationDbContext = dbContext;
		}
		public async Task<bool> IsUserNameDuplicateAsync(string name, Guid? id = null)
		{
			if (id.HasValue)
			{
				return (await GetCountAsync(x => x.Id != id.Value && x.Name == name)) > 0;
			}
			else
			{
				return (await GetCountAsync(x => x.Name == name)) > 0;
			}
		}
		public async Task<List<Question>> GetQuestionByUserIdAsync(Guid userId)
		{
			return await _applicationDbContext.Questions.Where(q => q.UserId == userId).ToListAsync();
		}
	}
}
