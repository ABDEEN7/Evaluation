using AutoMapper;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Services.Mappers.Admin
{
    public class FormEvalMatrixProfile : Profile
    {
        public FormEvalMatrixProfile()
        {
            CreateMap<FormEvalMatrix, FormEvalMatrixDTO>()
        .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
        .ForMember(d => d.NameEn, opt => opt.MapFrom(src => src.NameEn))
        .ForMember(d => d.NameAr, opt => opt.MapFrom(src => src.NameAr))
        .ForMember(d => d.DepartmentId, opt => opt.MapFrom(src => src.DepartmentId))
        .ForMember(d => d.Department, opt => opt.MapFrom(src => src.Department.BackendName))
        .ForMember(d => d.Startdate, opt => opt.MapFrom(src => src.Startdate))
        .ForMember(d => d.EndDate, opt => opt.MapFrom(src => src.EndDate))
        .ForMember(dest => dest.UpdateBy, opt => opt.MapFrom<UserProfileResolver, Guid?>(src => src.UpdateById.HasValue ? src.UpdateById : src.CreateById))
        .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate.HasValue ? src.UpdateDate.Value.ToString("yyyy-MM-dd hh:mm:ss tt") : src.CreateDate.ToString("yyyy-MM-dd hh:mm:ss tt")))
        .ReverseMap();
        }
    }
}