using AutoMapper;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.SystemSetting;
using Evaluation.Services.Mappers.Admin;
using Evaluation.SharedHelper.Models.Admin;


namespace Evaluation.Services.Mappers
{
    public class UiControlProfile : Profile
    {
        public UiControlProfile()
        {
            CreateMap<UiControl, UiControlDTO>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive ?? true))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted ?? false))
                .ForMember(dest => dest.UpdateBy, opt => opt.MapFrom<UserProfileResolver, Guid?>(src => src.UpdateById.HasValue ? src.UpdateById : src.CreateById))
                .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate.HasValue ? src.UpdateDate.Value.ToString("yyyy-MM-dd hh:mm:ss tt") : src.CreateDate.ToString("yyyy-MM-dd hh:mm:ss tt")))
                 .ForMember(dest => dest.ArValue, opt => opt.MapFrom(src => src.ValueAr))
                   .ForMember(dest => dest.EnValue, opt => opt.MapFrom(src => src.ValueEn));
        }
    }
}
