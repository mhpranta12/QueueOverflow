using AutoMapper;
using QueueOverflow.Domain.Entities;
using QueueOverflow.Web.Areas.user.Models;

namespace QueueOverflow.Web
{
	public class WebProfile :Profile
	{
		public WebProfile() 
		{ 
			CreateMap<EditProfileModel,UserInfo>()
				.ReverseMap();
		}
	}
}
