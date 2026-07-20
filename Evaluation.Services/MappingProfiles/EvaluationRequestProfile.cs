using AutoMapper;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.DAL.Models.ServiceRequestEntities;
using Evaluation.SharedHelper.Dtos.PlanDto;
using Evaluation.SharedHelper.Models.Api.ServiceRequestEntitiesDTO;
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
			.ForMember(d => d.Color, opt => opt.MapFrom(src => src.ServiceStatus.ColorCode))//!src.ServiceStatus.ColorCode.IsNullOrEmpty() ? src.ServiceStatus.ColorCode : "#f85a40")
            .ForMember(d => d.Source, opt => opt.MapFrom(_ => "EvaluationRequest"))
			.ReverseMap();

		CreateMap<ServiceRequest, EvaluationRequestCalenderDto>()
			.ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
			.ForMember(d => d.ParentId, opt => opt.MapFrom(src => src.EvaluationRequestId))
			.ForMember(d => d.Title, opt => opt.MapFrom(src => src.RequestNumber))
			.ForMember(d => d.Start, opt => opt.MapFrom(src => src.VisitDateFrom!.Value.ToString("yyyy-MM-dd'T'HH:mm:ss")))
			.ForMember(d => d.End, opt => opt.MapFrom(src => src.VisitDateTo!.Value.ToString("yyyy-MM-dd'T'HH:mm:ss")))
			.ForMember(d => d.Color, opt => opt.MapFrom(src => src.Status.ColorCode))//!src.Status.ColorCode.IsNullOrEmpty() ? src.Status.ColorCode : "#dfff00")
            .ForMember(d => d.Source, opt => opt.MapFrom(_ => "ServiceRequest"))
		 .ReverseMap();

		CreateMap<EvaluationRequest, EvaluationRequestDTO>()
			.ForMember(d => d.RequestNumber, opt => opt.MapFrom(src => src.RequestNumber))
			 .ForMember(d => d.FromDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.FromDate)))
			.ForMember(d => d.ToDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.ToDate)))
			.ForMember(d => d.EvaluationDate, opt => opt.MapFrom(src => src.EvaluationDate))
			.ForMember(d => d.NextEvaluationDate, opt => opt.MapFrom(src => src.NextEvaluationDate))
			.ForMember(d => d.EvaluationResult, opt => opt.MapFrom<LocalizedEvaluationResultNameResolver>())
			.ForMember(d => d.Status, opt => opt.MapFrom<LocalizedEvaluationRequestStatusNameResolver>())
			.ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
			.ForMember(d => d.AcademicYearId, opt => opt.MapFrom(src => src.Plan.AcademicYearId));

	}

}
public class LocalizedEvaluationRequestStatusNameResolver : IValueResolver<EvaluationRequest, EvaluationRequestDTO, string>
{
	public string Resolve(EvaluationRequest src, EvaluationRequestDTO dest, string destMember, ResolutionContext context)
	{
		var lang = context.Items["lang"]?.ToString();
		return lang == "ar" ? src.ServiceStatus?.NameAr : src.ServiceStatus?.NameEn;
	}
}

public class LocalizedEvaluationResultNameResolver : IValueResolver<EvaluationRequest, EvaluationRequestDTO, string>
{
	public string Resolve(EvaluationRequest src, EvaluationRequestDTO dest, string destMember, ResolutionContext context)
	{
		var lang = context.Items["lang"]?.ToString();
		return lang == "ar" ? src.FormEvalMatrixValue?.NameAr : src.FormEvalMatrixValue?.NameEn;
	}
}