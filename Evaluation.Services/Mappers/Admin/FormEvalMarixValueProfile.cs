using AutoMapper;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.Services.MappingProfiles;
using Evaluation.SharedHelper.Models.Admin;



namespace Evaluation.Services.Mappers.Admin
{
    public class FormEvalMarixValueProfile : Profile
    {
        public FormEvalMarixValueProfile()
        {
            CreateMap<FormEvalMarixValue, FormEvalMarixValueDTO>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                 .ForMember(dest => dest.UpdateBy, opt => opt.MapFrom<UserProfileResolver, Guid?>(src => src.UpdateById.HasValue ? src.UpdateById : src.CreateById))
                .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.UpdateDate.HasValue ? src.UpdateDate.Value.ToString("yyyy-MM-dd hh:mm:ss tt") : src.CreateDate.ToString("yyyy-MM-dd hh:mm:ss tt")))
                .ForMember(dest => dest.FormEvalMatrix, opt => opt.MapFrom<FormEvalMatrixResolver, Guid?>(src => src.FormEvalMatrixId));

        }

    }


   

}
