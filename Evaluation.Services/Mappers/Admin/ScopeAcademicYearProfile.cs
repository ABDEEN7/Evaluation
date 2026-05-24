using AutoMapper;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.SharedHelper.Models.Admin;





namespace Evaluation.Services.Mappers.Admin
{
    public class ScopeAcademicYearProfile : Profile
    {
        public ScopeAcademicYearProfile()
        {
            CreateMap<ScopeAcademicYear, ScopeAcademicYearDTO>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.DepartmentId))
                .ForMember(dest => dest.scopeAcademicYearScopeParent, opt => opt.MapFrom(src => src.ScopeParentId))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                .ForMember(dest => dest.UpdateBy, opt => opt.MapFrom<UserProfileResolver, Guid?>(src => src.UpdateById.HasValue ? src.UpdateById : src.CreateById))
               .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate.HasValue ? src.UpdateDate.Value.ToString("yyyy-MM-dd hh:mm:ss tt") : src.CreateDate.ToString("yyyy-MM-dd hh:mm:ss tt")))
                 .ForMember(dest => dest.Scope, opt => opt.MapFrom(src => src.ScopeId))
                 .ForMember(dest => dest.AcademicYear, opt => opt.MapFrom(src => src.AcademicYearId));
        }

    }



}
