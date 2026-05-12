using AutoMapper;
using Evaluation.DAL.Dtos.Form;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.SharedHelper.Dtos.Form;

namespace Evaluation.Services.MappingProfiles;

public class FormProfile : Profile
{
    public FormProfile()
    {
        CreateMap<FormItem, FormItemDto>()
            .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(d => d.Name, opt => opt.MapFrom(src => src.NameAr))
            //.ForMember(d => d.OrderNo, opt => opt.MapFrom(src => src.OrderNo))
            .ForMember(d => d.HasNote, opt => opt.MapFrom(src => src.HasNote))
            .ForMember(d => d.NoteRequired, opt => opt.MapFrom(src => src.NoteRequired))
            .ForMember(d => d.SubFormItems, opt => opt.MapFrom(src => src.SubFormItems))
            .ForMember(d => d.HasMultiEvaluation, opt => opt.MapFrom(src => src.HasMuliEvaluation))
            //.ForMember(d => d.RelatedItemName, opt => opt.MapFrom(src => src.RelatedFrom.FirstOrDefault().RelatedItem.NameAr))
            //.ForMember(d => d.RelatedItemId, opt => opt.MapFrom(src => src.RelatedFrom.FirstOrDefault().RelatedItemId))
            .ReverseMap();

        CreateMap<SubFormItem, SubFormItemDto>()
            .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(d => d.Name, opt => opt.MapFrom(src => src.NameAr))
            .ReverseMap();

        CreateMap<FormItemValue, FormItemEvaluationDto>()
          .ForMember(d => d.Value, opt => opt.MapFrom(src => src.ActualValue))
          .ForMember(d => d.Note, opt => opt.MapFrom(src => src.Note))
          .ForMember(d => d.Id, opt => opt.MapFrom(src => src.FormItemId))
          .ForMember(d => d.ValueId, opt => opt.MapFrom(src => src.Id))
          .ForMember(d => d.Name, opt => opt.MapFrom(src => src.RenameItem))
          .ReverseMap();


        CreateMap<FormItemValue, RelatedItemDto>()
          .ForMember(d => d.Value, opt => opt.MapFrom(src => src.ActualValue))
          .ForMember(d => d.Note, opt => opt.MapFrom(src => src.Note))
          .ForMember(d => d.Id, opt => opt.MapFrom(src => src.FormItemId))
          .ReverseMap();

        CreateMap<SubFormItemValue, SubFormItemEvaluationDto>()
         .ForMember(d => d.Value, opt => opt.MapFrom(src => src.FieldDropDownValueId))
         .ForMember(d => d.Note, opt => opt.MapFrom(src => src.Note))
         .ForMember(d => d.Id, opt => opt.MapFrom(src => src.SubFormItemId))
         .ForMember(d => d.ValueId, opt => opt.MapFrom(src => src.Id))
         .ReverseMap();

        CreateMap<FormEvaluationDto, FormEvaluationValue>()
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


        CreateMap<FormEvalMatrixValue, FormEvalMarixValueDto>()
         .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
         .ForMember(d => d.Name, opt => opt.MapFrom(src => src.NameEn))
         .ForMember(d => d.MaxValue, opt => opt.MapFrom(src => src.MaxValue))
         .ForMember(d => d.MinValue, opt => opt.MapFrom(src => src.MinValue))
         .ForMember(d => d.ActualMatrixValue, opt => opt.MapFrom(src => src.ActualMatrixValue))
         .ReverseMap();

    }
}
