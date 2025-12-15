using AutoMapper;
using Evaluation.DAL.Models.Planing;
using Evaluation.DAL.Models.Planing.EvaluationRequestEntity;
using Evaluation.SharedHelper.Dtos.PlanDto;
using Evaluation.SharedHelper.Dtos.SchoolDto;

namespace Evaluation.Services.Mappers.WebApp;

internal class PlanProfiller : Profile
{
    public PlanProfiller()
    {
        CreateMap<Plan, PlanDetailsDto>()
             .ForMember(dest => dest.Name,
                 opt => opt.MapFrom(src => src.PlanName))

               .ForMember(dest => dest.PlanTypeId,
                opt => opt.MapFrom(src => src.PlanTypeDepId))

            .ForMember(dest => dest.Schools,
                opt => opt.MapFrom(src => src.EvaluationRequests));

        CreateMap<EvaluationRequest, SelectedSchool>()
          .ForMember(dest => dest.VisitTypeId,
              opt => opt.MapFrom(src => src.ServiceStatusId))

          .ForMember(dest => dest.StartEvaluationDate,
              opt => opt.MapFrom(src => src.FromDate))

          .ForMember(dest => dest.EndEvaluationDate,
              opt => opt.MapFrom(src => src.ToDate));
    }
}
