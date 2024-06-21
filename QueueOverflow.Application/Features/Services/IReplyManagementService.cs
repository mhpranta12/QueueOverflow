using QueueOverflow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace QueueOverflow.Application.Features.Services
{
    public interface IReplyManagementService
    {
		Task CreateReplyAsync(string text,string userName,Guid questionId,Guid userId);
		Task <List<Reply>> GetRepliesOfQuestionAsync(Guid questionId);
	}
}
