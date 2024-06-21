using Autofac;
using QueueOverflow.Application.Features.Services;
using QueueOverflow.Domain.Entities;

namespace QueueOverflow.Web.Models
{
    public class ViewQuestionsModel
    {
		private IQuestionManagementService _questionManagementService;
		private ILifetimeScope _scope;
		public IList<Question> Questions { get; set; }
		public string? SearchTitle { get; set; }
		public double QuestionsCount { get; set; }
		public ViewQuestionsModel() { }
		public ViewQuestionsModel(IQuestionManagementService questionManagementService,
			ILifetimeScope scope)
		{
			_questionManagementService = questionManagementService;
			_scope = scope;
		}
		public async Task GetAllQuestionsAsync()
		{
			Questions = new List<Question>();
			Questions = await _questionManagementService.GetAllQuestionsAsync();
			QuestionsCount = Questions.Count();
		}
		public async Task SearchQuestionsAsync()
		{
			Questions = new List<Question>();
			Questions = await _questionManagementService.SearchQuestions(SearchTitle);
			QuestionsCount = Questions.Count();
		}
		internal void Resolve(ILifetimeScope scope)
		{
			_scope = scope;
			_questionManagementService = _scope.Resolve<IQuestionManagementService>();
		}
	}
}
