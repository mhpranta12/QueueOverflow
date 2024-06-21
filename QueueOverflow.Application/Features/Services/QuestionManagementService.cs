using QueueOverflow.Domain.Entities;
using QueueOverflow.Domain.Exceptions;
using QueueOverflow.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QueueOverflow.Application.Features.Services
{
    public class QuestionManagementService:IQuestionManagementService
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public QuestionManagementService(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateQuestionAsync(string title, string description, string tags,string userName, Guid userId)
        {
			Guid questionId = Guid.NewGuid();
			bool isDuplicateTitle = await _unitOfWork.QuestionRepository.
				IsTitleDuplicateAsync(title);

			if (isDuplicateTitle)
				throw new DuplicateQuestionTitleException();

			List<Tag> TagsObjects = new List<Tag>();
            string[] splittedTags = tags.Split();
			//extra validation 
			if (splittedTags.Length < 4)
			{
				foreach (var tag in splittedTags)
				{
					if (!string.IsNullOrWhiteSpace(tag))
					{
						Tag tagObject = new Tag()
						{
							Id = Guid.NewGuid(),
							Name = tag,
							QuestionId = questionId,
						};
						TagsObjects.Add(tagObject);
					}
				}
				Question question = new Question()
				{
					Id = questionId,
					Title = title,
					Description = description,
					CreationTime = DateTime.Now,
					Like = 0,
					Dislike = 0,
					Tags = TagsObjects,
					UserName = userName,
					UserId = userId
				};
				await _unitOfWork.QuestionRepository.AddAsync(question);
				await _unitOfWork.SaveAsync();
			}
			else
				throw new TagExceedsException();

		}

        public async Task<IList<Question>> GetAllQuestionsAsync()
        {
           return await _unitOfWork.QuestionRepository.GetAllQuestionsAsync();
        }
        public  Question GetQuestionById(Guid Id)
        {
           return  _unitOfWork.QuestionRepository.GetQuestionById(Id);
        }
		public async Task DeleteQuestionById(Guid Id)
		{
			try
			{
				var question = await _unitOfWork.QuestionRepository.GetByIdAsync(Id);
				await _unitOfWork.QuestionRepository.RemoveAsync(question);
				await _unitOfWork.SaveAsync();
			}
			catch(Exception ex)
			{
                await Console.Out.WriteLineAsync(" "+ex);
            }
		}

		public async Task<List<Question>> GetQuestionsByTagIdAsync(Guid tagId)
        {
            return await _unitOfWork.QuestionRepository.GetQuestionsByTagIdAsync(tagId);
        }

		public async Task LikeQuestionAsync(Guid id)
		{
			var question =  await _unitOfWork.QuestionRepository.GetByIdAsync(id);
			if(question is { })
			{
				question.Like += 1;
				await _unitOfWork.QuestionRepository.EditAsync(question);
				await _unitOfWork.SaveAsync();
			}
		}
		public async Task UnLikeQuestionAsync(Guid id)
		{
			var question = await _unitOfWork.QuestionRepository.GetByIdAsync(id);
			if (question is { })
			{
				question.Dislike += 1;
				await _unitOfWork.QuestionRepository.EditAsync(question);
				await _unitOfWork.SaveAsync();
			}
		}
		public async Task<IList<Question>> SearchQuestions(string title)
        {
           IList<Question> questions = await _unitOfWork.QuestionRepository.GetAllQuestionsAsync();
           //Comparing by ignoring case for a better comparison
			questions = questions.Where(q=>q.Title.Contains(title,StringComparison.OrdinalIgnoreCase)).ToList();
           return questions;
        }
	}
}
