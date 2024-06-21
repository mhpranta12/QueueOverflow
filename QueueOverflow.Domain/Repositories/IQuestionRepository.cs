using QueueOverflow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueOverflow.Domain.Repositories
{
    public interface IQuestionRepository: IRepositoryBase<Question,Guid>
    {
		Task<IList<Question>> GetAllQuestionsAsync();
		Question GetQuestionById(Guid id);
		Task<List<Question>> GetQuestionsByTagIdAsync(Guid tagId);
		Task<bool> IsTitleDuplicateAsync(string title, Guid? id = null);
	}
}
