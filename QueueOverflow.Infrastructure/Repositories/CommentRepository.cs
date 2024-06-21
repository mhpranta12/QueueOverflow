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
    public class CommentRepository : Repository<Comment, Guid>, ICommentRepository
    {
        private IApplicationDbContext _applicationDbContext;
        public CommentRepository(IApplicationDbContext dbContext) : base((DbContext)dbContext)
        {
            _applicationDbContext = dbContext;
        }
        public async Task<List<Comment>> GetCommentsOfQuestionAsync(Guid questionId)
        {
            return await _applicationDbContext.Comments.Where(q => q.QuestionId == questionId).ToListAsync();
        }
    }
}
