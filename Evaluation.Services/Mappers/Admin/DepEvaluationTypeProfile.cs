using AutoMapper;
using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.DAL.Models.Master;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Services.Mappers.Admin;

public class DepEvaluationTypeProfile : Profile
{
    public DepEvaluationTypeProfile()
    {
        CreateMap<DepEvaluationType, DepEvaluationTypeDto>();
        CreateMap<DepEvaluationType, ResponseDepEvaluationTypeDto>()
            .ForMember(x => x.Department, opt => opt.MapFrom(src => src.Department.BackendName))
            .ForMember(s => s.EvaluationType, opt => opt.MapFrom(src => src.EvaluationType.BackendName))
            .ForMember(dest => dest.UpdateBy, opt => opt.MapFrom<UserProfileResolver, Guid?>(src => src.UpdateById.HasValue ? src.UpdateById : src.CreateById));
    }
}
