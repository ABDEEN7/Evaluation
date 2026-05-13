using AutoMapper;
using Evaluation.DAL.Models.UserEntiy;
using Evaluation.SharedHelper.Models.Api.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.Services.Mappers.WebApp
{
	public class UserProfileProfile : Profile
	{
		public UserProfileProfile()
		{
			CreateMap<MinistryUser, UserProfileDTO>()
			  .ForMember(dest => dest.JobDescription, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.JobTitleAr) ? "" : src.JobTitleAr))
			  .ForMember(dest => dest.PrefferedLang, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.PreferredLanguage) ? "" : src.PreferredLanguage))
			 .ForMember(dest => dest.Email, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.Email) ? "" : src.Email))
			 .ForMember(dest => dest.QID, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.QID) ? "" : src.QID))
			 .ForMember(dest => dest.FullNameAr, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.NameAr) ? "" : src.NameAr))
			 .ForMember(dest => dest.FullNameEn, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.NameEn) ? "" : src.NameEn))
			 .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.Mobile) ? "" : src.Mobile))
			 .ForMember(dest => dest.NationalityCode, opt => opt.MapFrom(src => src.NationalityCode))
			 .ForMember(dest => dest.LastLoginDate, opt => opt.MapFrom(src => src.LastLoginDate == null ? "" : src.LastLoginDate.ToString()));
		}

		public class UserProfileTypeResolver : IValueResolver<MinistryUser, UserProfileDTO, string>
		{
			public string Resolve(MinistryUser source, UserProfileDTO destination, string destMember, ResolutionContext context)
			{
				return source is MinistryUser ? "minstry" : "student";
			}
		}
	}
}
