using AutoMapper;
using Evaluation.DAL.Models.EvalResult;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.SharedHelper.Models.Admin;

namespace Evaluation.Services.Mappers.Admin;

public class DepEvalMatrixProfile : Profile
{
    public DepEvalMatrixProfile()
    {
        CreateMap<DepEvalMatrix, DepEvalMatrixDTO>()
    .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
    .ForMember(d => d.NameEn, opt => opt.MapFrom(src => src.NameEn))
    .ForMember(d => d.NameAr, opt => opt.MapFrom(src => src.NameAr))
    .ForMember(d => d.AcademicYearId, opt => opt.MapFrom(src => src.AcademicYearId))
    .ForMember(d => d.AcademicYear, opt => opt.MapFrom(src => src.AcademicYear.NameEn))
    .ForMember(d => d.ItemValue, opt => opt.MapFrom(src => src.ItemValue))
    .ForMember(d => d.MaxValue, opt => opt.MapFrom(src => src.MaxValue))
    .ForMember(d => d.MinValue, opt => opt.MapFrom(src => src.MinValue))
    .ForMember(d => d.NextEvaluationDays, opt => opt.MapFrom(src => src.NextEvaluationDays))
    .ForMember(d => d.NextFollowUpDays, opt => opt.MapFrom(src => src.NextFollowUpDays))
    .ForMember(dest => dest.UpdateBy, opt => opt.MapFrom<UserProfileResolver, Guid?>(src => src.UpdateById.HasValue ? src.UpdateById : src.CreateById))
    .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate.HasValue ? src.UpdateDate.Value.ToString("yyyy-MM-dd hh:mm:ss tt") : src.CreateDate.ToString("yyyy-MM-dd hh:mm:ss tt")))
    .ReverseMap();
    }
}