using QueueOverflow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueOverflow.Application.Features.Services
{
    public class ReplyManagementService :IReplyManagementService
    {
        private readonly IApplicationUnitOfWork _unitOfWork;
        public ReplyManagementService(IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateReplyAsync(string text, string userName, Guid questionId, Guid userId)
        {
            var userInfo = await _unitOfWork.UserInfoRepository.GetByIdAsync(userId);

            Reply reply = new Reply()
            {
                Id = Guid.NewGuid(),
                Text = text,
                UserName = userName,
                CreationTime = DateTime.Now,
                QuestionId = questionId,
                UserId = userId
            };
            await _unitOfWork.ReplyRepository.AddAsync(reply);
            await _unitOfWork.SaveAsync();
        }
		public async Task<List<Reply>> GetRepliesOfQuestionAsync(Guid questionId)
		{
            return await _unitOfWork.ReplyRepository.GetRepliesOfQuestionAsync(questionId);
		}
	}
}
