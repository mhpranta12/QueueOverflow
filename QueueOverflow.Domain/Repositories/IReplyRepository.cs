using QueueOverflow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueOverflow.Domain.Repositories
{
    public interface IReplyRepository:IRepositoryBase<Reply, Guid>
    {
        Task<List<Reply>> GetRepliesOfQuestionAsync(Guid questionId);
    }
}
