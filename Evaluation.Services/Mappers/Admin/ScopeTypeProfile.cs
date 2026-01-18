using AutoMapper;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Services.Mappers.Admin;

public class ScopeTypeProfile : Profile
{
    public ScopeTypeProfile()
    {
        CreateMap<ScopeType, ScopeTypeDTO>()
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive ?? true))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted ?? false))
             .ForMember(dest => dest.UpdateBy, opt => opt.MapFrom<UserProfileResolver, Guid?>(src => src.UpdateById.HasValue ? src.UpdateById : src.CreateById))
            .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate.HasValue ? src.UpdateDate.Value.ToString("yyyy-MM-dd hh:mm:ss tt") : src.CreateDate.ToString("yyyy-MM-dd hh:mm:ss tt")))
             .ForMember(dest => dest.Department, opt => opt.MapFrom<DepartmentResolver, Guid?>(src => src.DepartmentId))
              .ForMember(dest => dest.Parent, opt => opt.MapFrom<ScopeTypeResolver, Guid?>(src => src.ParentId));

    }

}