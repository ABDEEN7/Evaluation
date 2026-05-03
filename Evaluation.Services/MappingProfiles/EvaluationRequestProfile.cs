using AutoMapper;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.DAL.Models.ServiceEnities;
using Evaluation.DAL.Models.ServiceRequestEntities;
using Evaluation.SharedHelper.Dtos.PlanDto;
using Microsoft.IdentityModel.Tokens;


namespace Evaluation.Services.MappingProfiles;



public class EvaluationRequestProfile : Profile
{
    public EvaluationRequestProfile()
    {
        CreateMap<EvaluationRequest, EvaluationRequestCalenderDto>()
            .ForMember(d => d.Title, opt => opt.MapFrom(src => src.OrgTree.NameEn + $" ({src.Plan.PlanName}) - {src.DepEvaluationType.NameEn}"))
            .ForMember(d => d.Start, opt => opt.MapFrom(src => src.FromDate.ToString("yyyy-MM-dd")))
            .ForMember(d => d.End, opt => opt.MapFrom(src => src.ToDate.AddDays(1).ToString("yyyy-MM-dd")))//here we add extra day to show it in the calender corectly
            .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(d => d.Color, opt => opt.MapFrom(src => !src.ServiceStatus.ColorCode.IsNullOrEmpty()? src.ServiceStatus.ColorCode : "#f85a40"))
            .ForMember(d => d.Source, opt => opt.MapFrom(_ => "EvaluationRequest"))
            .ForMember(d => d.Url, opt => opt.MapFrom(_ => "https://www.google.com"))
            .ReverseMap();

        CreateMap<ServiceRequest, EvaluationRequestCalenderDto>()
            .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(d => d.ParentId, opt => opt.MapFrom(src => src.EvaluationRequestId))
            .ForMember(d => d.Title, opt => opt.MapFrom(src => src.RequestNumber))
            .ForMember(d => d.Start, opt => opt.MapFrom(src => src.VisitDateFrom!.Value.ToString("yyyy-MM-dd'T'HH:mm:ss")))
            .ForMember(d => d.End, opt => opt.MapFrom(src => src.VisitDateTo!.Value.ToString("yyyy-MM-dd'T'HH:mm:ss")))
            .ForMember(d => d.Color, opt => opt.MapFrom(src => !src.Status.ColorCode.IsNullOrEmpty()? src.Status.ColorCode : "#dfff00"))
            .ForMember(d => d.Source, opt => opt.MapFrom(_ => "ServiceRequest"))
            .ForMember(d => d.Url, opt => opt.MapFrom(_ => "https://www.google.com"))
         .ReverseMap();

    }

}