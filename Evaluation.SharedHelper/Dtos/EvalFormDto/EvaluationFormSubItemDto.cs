using Evaluation.SharedHelper.Models;

namespace Evaluation.DAL.Models.FormsModules
{
    public class EvaluationFormSubItemDto : EntityBaseDTO
    {
        public Guid FormItemId { get; set; }
        public string? FormItem { get; set; }
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;

        public bool IsOption { get; set; }
        public bool HasNote { get; set; }
        public bool NoteRequired { get; set; }
        public int OrderNo { get; set; }

        public Guid? DropDownTypeId { get; set; }
        public string? DropDownType { get; set; }
    }
}
