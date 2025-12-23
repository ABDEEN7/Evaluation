using AutoMapper;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.SharedHelper.Dtos.PlanDto;


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
            .ReverseMap();
    }
}
