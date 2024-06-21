using Autofac;
using QueueOverflow.Application.Features.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueOverflow.Application
{
    public class ApplicationModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<QuestionManagementService>().As<IQuestionManagementService>()
                 .InstancePerLifetimeScope();
            builder.RegisterType<UserInfoManagementService>().As<IUserInfoManagementService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ReplyManagementService>().As<IReplyManagementService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<TagManagementService>().As<ITagManagementService>()
                .InstancePerLifetimeScope(); 
            builder.RegisterType<CommentManagementService>().As<ICommentManagementService>()
                .InstancePerLifetimeScope(); 
        }
    }
}
