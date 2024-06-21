using QueueOverflow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueOverflow.Application.Features.Services
{
	public class CommentManagementService : ICommentManagementService
	{
		private readonly IApplicationUnitOfWork _unitOfWork;
		public CommentManagementService(IApplicationUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public async Task CreateCommentAsync(string text, Guid userId, Guid questionId, string userName)
		{
			uint usersPoints=0;
            var userInfo = await _unitOfWork.UserInfoRepository.GetByIdAsync(userId);
			if(userInfo is { })
			{
				usersPoints = userInfo.Reputations;
				if (usersPoints > 50)
				{
					Comment comment = new Comment()
					{
						Id = Guid.NewGuid(),
						Text = text,
						CreationTime = DateTime.Now,
						UserName = userName,
						UserId = userId,
						QuestionId = questionId,
					};
					await _unitOfWork.CommentRepository.AddAsync(comment);
					await _unitOfWork.SaveAsync();
				}
			}
		}

        public async Task<List<Comment>> GetCommentsOfQuestionAsync(Guid questionId)
        {
			return await _unitOfWork.CommentRepository.GetCommentsOfQuestionAsync(questionId);
        }
    }
}
