using QueueOverflow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueOverflow.Application.Features.Services
{
    public interface IQuestionManagementService
    {
        Task CreateQuestionAsync(string title,string description, string tags,string userName, Guid userId);
        Task<IList<Question>> GetAllQuestionsAsync();
        Question GetQuestionById(Guid id);
        Task<IList<Question>> SearchQuestions(string title);
		Task LikeQuestionAsync(Guid id);
		Task UnLikeQuestionAsync(Guid id);
        Task DeleteQuestionById(Guid Id);
		Task<List<Question>> GetQuestionsByTagIdAsync(Guid tagId);
	}
}
