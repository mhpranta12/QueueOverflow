using QueueOverflow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueOverflow.Domain.Repositories
{
    public interface ICommentRepository :IRepositoryBase<Comment,Guid>
    {
        Task<List<Comment>> GetCommentsOfQuestionAsync(Guid questionId);
    }
}
