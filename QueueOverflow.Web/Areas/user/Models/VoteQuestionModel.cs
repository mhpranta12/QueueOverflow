using Autofac;
using QueueOverflow.Application.Features.Services;

namespace QueueOverflow.Web.Areas.user.Models
{
	public class VoteQuestionModel
	{
		private IQuestionManagementService _questionManagementService;
		private ILifetimeScope _scope;
		public VoteQuestionModel() { }
		public VoteQuestionModel(IQuestionManagementService questionManagementService,
			ILifetimeScope scope)
		{
			_questionManagementService = questionManagementService;
			_scope = scope;
		}
		public Guid? Id { get; set; }

		public async Task LikeQuestionAsync(Guid id)
		{
			await _questionManagementService.LikeQuestionAsync(id);
		}
		public async Task UnLikeQuestionAsync(Guid id)
		{
			await _questionManagementService.UnLikeQuestionAsync(id);
		}
		internal void Resolve(ILifetimeScope scope)
		{
			_scope = scope;
			_questionManagementService = _scope.Resolve<IQuestionManagementService>();
		}
	}
}
