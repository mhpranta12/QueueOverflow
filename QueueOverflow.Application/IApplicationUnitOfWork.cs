using QueueOverflow.Domain;
using QueueOverflow.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueOverflow.Application
{
    public interface IApplicationUnitOfWork : IUnitOfWork
    {
        IUserInfoRepository UserInfoRepository { get; }
        IQuestionRepository QuestionRepository { get; }
        ICommentRepository CommentRepository { get; }
        IReplyRepository ReplyRepository { get; }
        ITagRepository TagRepository { get; }
    }
}
