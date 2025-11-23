using AutoMapper;
using Evaluation.DAL.Dtos;
using Evaluation.DAL.Models.FormsModules;

namespace Evaluation.Services.MappingProfiles;

public class FormProfile: Profile
{
    public FormProfile()
    {
        CreateMap<FormItem, FormItemDto>()
            .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(d => d.Name, opt => opt.MapFrom(src => src.NameEn))
            .ReverseMap();
    }
}
