using AutoMapper;
using Evaluation.DAL.Dtos;
using Evaluation.DAL.Models.Planing;
using Evaluation.SharedHelper.Dtos.PlanDto;

namespace Evaluation.Services.MappingProfiles;

public class PlanProfile : Profile
{
    public PlanProfile()
    {
        CreateMap<Plan, PlanDto>()
            .ForMember(d => d.Name, opt => opt.MapFrom(src => src.PlanName))
            .ReverseMap();

        CreateMap<PlanDto, Plan>()
            .ForMember(d => d.PlanName, opt => opt.MapFrom(src => src.Name))
            .ReverseMap();
        CreateMap<CreateEvaluationPlanDto, Plan>()
            .ForMember(d => d.PlanName, opt => opt.MapFrom(src => src.Name))
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.CreateDate, o => o.Ignore())
            .ForMember(d => d.CreateById, o => o.Ignore())
            .ForMember(d => d.IsDeleted, o => o.Ignore());
    }
}