using Autofac;
using QueueOverflow.Application.Features.Services;
using QueueOverflow.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace QueueOverflow.Web.Areas.user.Models
{
	public class ViewSpecificQuestionModel
	{
        private IQuestionManagementService _questionManagementService;
        private IUserInfoManagementService _userInfoManagementService;
        private IReplyManagementService _replyManagementService;
        private ICommentManagementService _commentManagementService;
        private ILifetimeScope _scope;
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public Question? Question { get; set; }
        public Reply? ReplyToPost { get; set; }
        public Comment? CommentToPost { get; set; }
        public IList<Reply>? Replies { get; set; }
        public IList<Comment>? Comments { get; set; }
        public string? ReturnURL { get; set; }
        public ViewSpecificQuestionModel() { }
        public ViewSpecificQuestionModel(IQuestionManagementService questionManagementService,
            IReplyManagementService replyManagementService,
            IUserInfoManagementService userInfoManagementService,
			ICommentManagementService commentManagementService,
            ILifetimeScope scope)
        {
            _questionManagementService = questionManagementService;
            _replyManagementService = replyManagementService;
            _userInfoManagementService = userInfoManagementService;
            _commentManagementService = commentManagementService;
            _scope = scope;
        }
        public void GetQuestionById(Guid Id)
        {
            Question = _questionManagementService.GetQuestionById(Id);
        }

		public async Task AttachUserIdAndNameAsync(Guid Id)
		{
			UserId = Id;
            var userInfo = await _userInfoManagementService.GetUserAsync(Id);
            if (userInfo.Name is not null)
            {
				UserName = userInfo.Name;
			}
		}
        public async Task LoadRepliesAsync(Guid questionId)
        {
           Replies = await _replyManagementService.GetRepliesOfQuestionAsync(questionId);
        }
        public async Task LoadCommentsAsync(Guid questionId)
        {
           Comments = await _commentManagementService.GetCommentsOfQuestionAsync(questionId);
        }
		
        public async Task CreateReplyAsync()
        {
           await _replyManagementService.CreateReplyAsync(ReplyToPost.Text,ReplyToPost.UserName,ReplyToPost.QuestionId,ReplyToPost.UserId);
           await _userInfoManagementService.AddAnswerGivenAsync(UserId);
        }
        public async Task<bool> CanCommentAsync()
        {
            uint points = await _userInfoManagementService.CheckUsersPointAsync(UserId);
            return points > 50;
        }
        public async Task CreateCommentAsync()
        {
            await _commentManagementService.CreateCommentAsync(CommentToPost.Text, CommentToPost.UserId
                , CommentToPost.QuestionId,CommentToPost.UserName);
        }
        public async Task UpdatePointOnReplyAsync()
        {
            await _userInfoManagementService.UpdateUsersReputationsAsync(UserId, 5);
        }
        public async Task UpdatePointOnCommentAsync()
        {
            await _userInfoManagementService.UpdateUsersReputationsAsync(UserId, 10);
        }
		internal void Resolve(ILifetimeScope scope)
		{
			_scope = scope;
			_questionManagementService = _scope.Resolve<IQuestionManagementService>();
			_replyManagementService = _scope.Resolve<IReplyManagementService>();
			_userInfoManagementService = _scope.Resolve<IUserInfoManagementService>();
			_commentManagementService = _scope.Resolve<ICommentManagementService>();
		}
	}
}
