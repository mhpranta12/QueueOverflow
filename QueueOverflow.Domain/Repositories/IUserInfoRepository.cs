using QueueOverflow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueOverflow.Domain.Repositories
{
    public interface IUserInfoRepository: IRepositoryBase<UserInfo,Guid>
    {
		Task<List<Question>> GetQuestionByUserIdAsync(Guid userId);
		Task<bool> IsUserNameDuplicateAsync(string name, Guid? id = null);

	}
}
