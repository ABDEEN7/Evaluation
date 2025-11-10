
using Evaluation.SharedHelper.Models.Api.FormBuilderDTO;


namespace Evaluation.SharedHelper.Models.Api.ActionEntitiesDTOs
{
    public class StepDTO : EntityBaseDTO
    {

        public string TitleAr { get; set; } = null!;
        public string TitleEn { get; set; } = null!;
        public Guid ServiceId { get; set; }
        public string Service { get; set; } = null!;
        public int? OrderNo { get; set; }
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public string? Icon { get; set; }
       
        public string? ColorCode { get; set; }
        public bool IsDefault { get; set; }
        public string? Type { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public List<FormGroupDTO> FormGroups { get; set; } = new();
        public List<string> SchAttachments { get; set; } = new();

    }
}
