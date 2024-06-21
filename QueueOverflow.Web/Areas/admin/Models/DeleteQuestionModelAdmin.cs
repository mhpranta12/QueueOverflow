using Autofac;
using QueueOverflow.Application.Features.Services;

namespace QueueOverflow.Web.Areas.admin.Models
{
	public class DeleteQuestionModelAdmin
	{
		private IQuestionManagementService _questionManagementService;
		private ILifetimeScope _scope;
		public DeleteQuestionModelAdmin() { }
		public DeleteQuestionModelAdmin(IQuestionManagementService questionManagementService,
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
