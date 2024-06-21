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
    public class ReplyRepository : Repository<Reply, Guid>,IReplyRepository
    {
		private IApplicationDbContext _applicationDbContext;
		public ReplyRepository(IApplicationDbContext dbContext) : base((DbContext)dbContext)
        {
			_applicationDbContext = dbContext;
		}

		public async Task<List<Reply>> GetRepliesOfQuestionAsync(Guid questionId)
		{
			return await _applicationDbContext.Replies.Where(r=>r.QuestionId == questionId).ToListAsync();
		}
	}
}
