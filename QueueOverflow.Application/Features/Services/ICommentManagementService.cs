using QueueOverflow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueOverflow.Application.Features.Services
{
	public interface ICommentManagementService
	{
		Task CreateCommentAsync(string text,Guid userId,Guid questionId, string userName);
        Task<List<Comment>> GetCommentsOfQuestionAsync(Guid questionId);
    }
}
