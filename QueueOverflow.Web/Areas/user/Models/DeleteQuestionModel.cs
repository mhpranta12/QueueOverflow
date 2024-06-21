using Autofac;
using QueueOverflow.Application.Features.Services;

namespace QueueOverflow.Web.Areas.user.Models
{
	public class DeleteQuestionModel
	{
		private IQuestionManagementService _questionManagementService;
		private ILifetimeScope _scope;
		public DeleteQuestionModel() { }
		public DeleteQuestionModel(IQuestionManagementService questionManagementService,
			ILifetimeScope scope)
		{
			_questionManagementService = questionManagementService;
			_scope = scope;
		}
		public Guid? Id { get; set; }

		public async Task RemoveQuestionAsync(Guid id)
		{
			await _questionManagementService.DeleteQuestionById(id);
		}
		internal void Resolve(ILifetimeScope scope)
		{
			_scope = scope;
			_questionManagementService = _scope.Resolve<IQuestionManagementService>();
		}
	}
}
