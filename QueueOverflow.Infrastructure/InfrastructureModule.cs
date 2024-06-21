using Autofac;
using QueueOverflow.Application;
using QueueOverflow.Application.Utilities;
using QueueOverflow.Domain.Repositories;
using QueueOverflow.Infrastructure.Email;
using QueueOverflow.Infrastructure.Repositories;
using QueueOverflow.Infrastructure.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueOverflow.Infrastructure
{
    public class InfrastructureModule: Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssembly;
        public InfrastructureModule(string connectionString, string migrationAssembly)
        {
            _connectionString = connectionString;
            _migrationAssembly = migrationAssembly;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationDbContext>().AsSelf()
                .WithParameter("connectionString", _connectionString)
                .WithParameter("migrationAssembly", _migrationAssembly)
                .InstancePerLifetimeScope();

            builder.RegisterType<ApplicationDbContext>().As<IApplicationDbContext>()
                .WithParameter("connectionString", _connectionString)
                .WithParameter("migrationAssembly", _migrationAssembly)
            .InstancePerLifetimeScope();

            builder.RegisterType<ApplicationUnitOfWork>().As<IApplicationUnitOfWork>()
                .InstancePerLifetimeScope();
            //Repositories Binding
            builder.RegisterType<UserInfoRepository>().As<IUserInfoRepository>()
                .InstancePerLifetimeScope(); 
            builder.RegisterType<QuestionRepository>().As<IQuestionRepository>()
                .InstancePerLifetimeScope(); 
            builder.RegisterType<ReplyRepository>().As<IReplyRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<CommentRepository>().As<ICommentRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<TagRepository>().As<ITagRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<HtmlEmailService>().As<IEmailService>()
               .InstancePerLifetimeScope();
        }
    }
}
