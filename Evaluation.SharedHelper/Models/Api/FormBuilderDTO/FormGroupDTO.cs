namespace Evaluation.SharedHelper.Models.Api.FormBuilderDTO
{
    public class FormGroupDTO : EntityBaseDTO
    {
        public string TitleAr { get; set; } = null!;
        public string TitleEn { get; set; } = null!;
        public string? BackendName { get; set; }
        public Guid ServiceId { get; set; }
        public string? Service { get; set; } = null!;
       
        public int Order { get; set; }

        public Guid FormGroupTypeId { get; set; }
        public string? FormGroupType { get; set; }
        public Guid? FormGroupCustomListId { get; set; }
        public string? FormGroupCustomList { get; set; }


        public List<FieldValueDTO> Fields { get; set; } = null!;
        public string FormGroupName { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}
