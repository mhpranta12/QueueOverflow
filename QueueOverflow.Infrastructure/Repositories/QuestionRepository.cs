using Microsoft.EntityFrameworkCore;
using QueueOverflow.Domain.Entities;
using QueueOverflow.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace QueueOverflow.Infrastructure.Repositories
{
    public class QuestionRepository : Repository<Question, Guid>,IQuestionRepository
    {
        private IApplicationDbContext _applicationDbContext;
        public QuestionRepository(IApplicationDbContext dbContext) : base((DbContext)dbContext)
        {
            _applicationDbContext = dbContext;
        }
		public async Task<bool> IsTitleDuplicateAsync(string title, Guid? id = null)
		{
			if (id.HasValue)
			{
				return (await GetCountAsync(x => x.Id != id.Value && x.Title == title)) > 0;
			}
			else
			{
				return (await GetCountAsync(x => x.Title == title)) > 0;
			}
		}
		public async Task<IList<Question>> GetAllQuestionsAsync()
        {
			return await _applicationDbContext.Questions.Include(q => q.Tags).ToListAsync();
		}
        public  Question GetQuestionById(Guid id)
        {
			return _applicationDbContext.Questions.Where(q => q.Id == id).Include(qt => qt.Tags).ToList()[0];
        }

		public async Task<List<Question>> GetQuestionsByTagIdAsync(Guid tagId)
		{
            List<Question> results = new List<Question>();
            string keyword = null;
			
			var questions = await _applicationDbContext.Questions.Include(q => q.Tags).ToListAsync();
			var matchingQuestions = questions.Where(q => q.Tags.Any(t => t.Id == tagId)).FirstOrDefault();
			var matchingTag = matchingQuestions.Tags.Where(t=>t.Id == tagId).FirstOrDefault();
			keyword = matchingTag.Name;
			if(keyword is not null)
			{
				results =  questions.Where(q=>q.Tags.Any(t=>t.Name==keyword)).ToList();
			}
            return results;
		}
	}
}
