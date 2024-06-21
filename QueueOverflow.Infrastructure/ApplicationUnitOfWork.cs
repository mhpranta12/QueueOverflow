using Microsoft.EntityFrameworkCore;
using QueueOverflow.Application;
using QueueOverflow.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueOverflow.Infrastructure
{
	public class ApplicationUnitOfWork : UnitOfWork, IApplicationUnitOfWork
	{
		public IUserInfoRepository UserInfoRepository { get; private set; }
		public IQuestionRepository QuestionRepository { get; private set; }
        public ICommentRepository CommentRepository { get; private set; }
		public IReplyRepository ReplyRepository { get; private set; }
        public ITagRepository TagRepository { get; private set; }


		public ApplicationUnitOfWork(IUserInfoRepository userInfoRepository, 
			IQuestionRepository questionRepository,
			ICommentRepository commentRepository,
			IReplyRepository replyRepository,
            ITagRepository tagRepository,
			IApplicationDbContext dbContext) : base((DbContext)dbContext)
		{
            UserInfoRepository = userInfoRepository;
			QuestionRepository = questionRepository;
			CommentRepository = commentRepository;
			ReplyRepository = replyRepository;
			TagRepository = tagRepository;
		}
	}
}
