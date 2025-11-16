using AutoMapper;
using Evaluation.DAL.Models.FormBuilder;
using Evaluation.SharedHelper.Models.Admin;



namespace Evaluation.Services.Mappers.Admin
{
    public class FieldViewConditionProfile : Profile
    {
        public FieldViewConditionProfile()
        {
            CreateMap<FieldViewCondition, FieldViewConditionDTO>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive ?? true))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted ?? false))
                 .ForMember(dest => dest.UpdateBy, opt => opt.MapFrom<UserProfileResolver, Guid?>(src => src.UpdateById.HasValue ? src.UpdateById : src.CreateById))
                .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate.HasValue ? src.UpdateDate.Value.ToString("yyyy-MM-dd hh:mm:ss tt") : src.CreateDate.ToString("yyyy-MM-dd hh:mm:ss tt")))
                .ForMember(dest => dest.FieldDropDownValueIds,
    opt => opt.MapFrom(src =>
        string.IsNullOrEmpty(src.FieldDropDownValueIds)
            ? Array.Empty<string>()
            : src.FieldDropDownValueIds.Split(",", StringSplitOptions.RemoveEmptyEntries)));

        }

    }
    
   
   
}
