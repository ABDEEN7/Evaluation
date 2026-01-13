using AutoMapper;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.Services.Mappers.Admin;
using Evaluation.SharedHelper.Dtos.EvalFormDto;

namespace Evaluation.Services.MappingProfiles;

public class FormScopeProfile : Profile
{
    public FormScopeProfile()
    {
        CreateMap<FormScope, FormScopeDTO>()
           .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive ?? true))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted ?? false))
                .ForMember(dest => dest.UpdateBy, opt => opt.MapFrom<UserProfileResolver, Guid?>(src => src.UpdateById.HasValue ? src.UpdateById : src.CreateById))
               .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate.HasValue ? src.UpdateDate.Value.ToString("yyyy-MM-dd hh:mm:ss tt") : src.CreateDate.ToString("yyyy-MM-dd hh:mm:ss tt")))
                .ForMember(dest => dest.Scope, opt => opt.MapFrom<ScopeResolver, Guid?>(src => src.ScopeId));
    }
}

