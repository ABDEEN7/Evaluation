using AutoMapper;
using Evaluation.DAL.Models.ActionEntities;
using Evaluation.SharedHelper.Models.Api.ActionEntitiesDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.Services.BusinessLayer.Mappers
{
	internal class ActionProfile : Profile
	{

		public ActionProfile()
		{
			CreateMap<ServiceAction, ActionCustomDTO>()
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NameAr ?? src.NameEn))
				.ForMember(dest => dest.NameAr, opt => opt.MapFrom(src => src.NameAr))
				.ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.NameEn))
				.ForMember(dest => dest.BakendName, opt => opt.MapFrom(src => src.BackendName))
				.ForMember(dest => dest.IsConfirmationAction, opt => opt.MapFrom(src => src.IsConfirmationAction))
				.ForMember(dest => dest.ConfirmationTitleAr, opt => opt.MapFrom(src => src.ConfirmationTitleAr))
				.ForMember(dest => dest.ConfirmationTitleEn, opt => opt.MapFrom(src => src.ConfirmationTitleEn))
				.ForMember(dest => dest.ConfirmationBodyAr, opt => opt.MapFrom(src => src.ConfirmationBodyAr))
				.ForMember(dest => dest.ConfirmationBodyEn, opt => opt.MapFrom(src => src.ConfirmationBodyEn))
				.ForMember(dest => dest.AllowDraft, opt => opt.MapFrom(src => src.AllowDraft))
				.ForMember(dest => dest.IsInitialAction, opt => opt.MapFrom(src => src.IsInitialAction))
				.ForMember(dest => dest.ActionTypeId, opt => opt.MapFrom(src => src.ActionTypeId))
				.ForMember(dest => dest.ServiceId, opt => opt.MapFrom(src => src.ServiceId))

				.ForMember(dest => dest.ActionType, opt => opt.MapFrom(src => src.ActionType))

				.ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.ConfirmationTitleEn))
				.ForMember(dest => dest.ActionTypeType, opt => opt.MapFrom(src => src.IsInitialAction ? "Initial" : "Other"));
		}
	}
}
