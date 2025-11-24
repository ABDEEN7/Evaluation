using AutoMapper;
using Evaluation.DAL.Dtos;
using Evaluation.DAL.Models.FormsModules;

namespace Evaluation.Services.MappingProfiles;

public class FormProfile : Profile
{
    public FormProfile()
    {
        CreateMap<FormItem, FormItemDto>()
            .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(d => d.Name, opt => opt.MapFrom(src => src.NameEn))
            .ForMember(d => d.OrderNo, opt => opt.MapFrom(src => src.OrderNo))
            .ForMember(d => d.HasNote, opt => opt.MapFrom(src => src.HasNote))
            .ForMember(d => d.SubFormItems, opt => opt.MapFrom(src => src.SubFormItems))
            .ReverseMap();

        CreateMap<SubFormItem, SubFormItemDto>()
            .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(d => d.Name, opt => opt.MapFrom(src => src.NameEn))
            .ReverseMap();
    }
}
