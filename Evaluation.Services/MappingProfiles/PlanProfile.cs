using AutoMapper;
using Evaluation.DAL.Dtos;
using Evaluation.DAL.Models.Planing;

namespace Evaluation.Services.MappingProfiles;

public class PlanProfile : Profile
{
    public PlanProfile()
    {
        CreateMap<Plan, PlanDto>()
            .ForMember(d => d.Name, opt => opt.MapFrom(src => src.PlanName))
            .ReverseMap();
    }
}