using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using QueueOverflow.Application.Features.Services;
using QueueOverflow.Infrastructure.Membership;
using QueueOverflow.Web.Areas.admin.Models;
using QueueOverflow.Web.Areas.user.Models;
using QueueOverflow.Web.Models;

namespace QueueOverflow.Web
{
    public class WebModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RegistrationModel>().AsSelf(); 
            builder.RegisterType<LoginModel>().AsSelf();
            builder.RegisterType<ViewProfileModel>().AsSelf(); 
            builder.RegisterType<EditProfileModel>().AsSelf();
            builder.RegisterType<CreateQuestionModel>().AsSelf();
            builder.RegisterType<CreateQuestionModel>().AsSelf();
            builder.RegisterType<ViewAllQuestionModel>().AsSelf();
            builder.RegisterType<ViewAllQuestionModelAdmin>();
			builder.RegisterType<ViewQuestionsModel>().AsSelf();
			builder.RegisterType<VoteQuestionModel>().AsSelf(); 
            builder.RegisterType<DeleteQuestionModelAdmin>().AsSelf();
			builder.RegisterType<DeleteQuestionModel>().AsSelf(); 
			builder.RegisterType<ViewSpecificQuestionModel>().AsSelf();
			builder.RegisterType<UpdateUserInfoModel>().AsSelf();
			builder.RegisterType<ConfirmEmailModel>().AsSelf();
            builder.RegisterType<Mapper>().As<IMapper>();
            builder.RegisterType<UserManager<ApplicationUser>>().AsSelf(); 
            builder.RegisterType<SignInManager<ApplicationUser>>().AsSelf();
			base.Load(builder);
        }
    }
}
