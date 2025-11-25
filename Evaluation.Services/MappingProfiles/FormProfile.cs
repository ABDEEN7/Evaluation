using AutoMapper;
using Evaluation.DAL.Dtos;
using Evaluation.DAL.Dtos.Form;
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

        CreateMap<FormItemValue, FormItemEvaluationDto>()
          .ForMember(d => d.Value, opt => opt.MapFrom(src => src.Value))
          .ForMember(d => d.Note, opt => opt.MapFrom(src => src.Note))
          .ForMember(d => d.Id, opt => opt.MapFrom(src => src.FormItemId))
          .ReverseMap();

        CreateMap<SubFormItemValue, SubFormItemEvaluationDto>()
         .ForMember(d => d.Value, opt => opt.MapFrom(src => src.FieldDropDownValueId))
         .ForMember(d => d.Note, opt => opt.MapFrom(src => src.Note))
         .ForMember(d => d.Id, opt => opt.MapFrom(src => src.SubFormItemId))
         .ReverseMap();

        CreateMap<FormEvaluationDto, FormEvaluationValueDto>()
              .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
              .ForMember(dest => dest.SubItems, opt => opt.Ignore()) // filled manually
              .AfterMap((src, dest, ctx) =>
              {
                  dest.SubItems = new List<SubFormItemValue>();

                  if (src.Items == null)
                      return;

                  // Flatten subitems and assign parent FormItemId
                  foreach (var item in src.Items)
                  {
                      if (item.SubItems == null)
                          continue;

                      foreach (var sub in item.SubItems)
                      {
                          var mappedSub = ctx.Mapper.Map<SubFormItemValue>(sub);
                          dest.SubItems.Add(mappedSub);
                      }
                  }
              });

    }
}
