

using Evaluation.SharedHelper.Models.Api.AttachmentsDTOs;
using Evaluation.SharedHelper.Models.Api.FormBuilderDTO;
using Evaluation.SharedHelper.Models.Api.TemplatesDTO;

namespace Evaluation.SharedHelper.Models.Api.ActionEntitiesDTOs
{
    public class ActionCustomDTO : EntityBaseDTO
    {
        public string? Name { get; set; }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string BakendName { get; set; } = null!;
        public bool IsConfirmationAction { get; set; }
        public string? ConfirmationTitleAr { get; set; }
        public string? ConfirmationTitleEn { get; set; }
        public string? ConfirmationBodyAr { get; set; }
        public string? ConfirmationBodyEn { get; set; }
        public bool AllowDraft { get; set; }
        public bool IsInitialAction { get; set; }
        public string? ActionTypeId { get; set; }
        public string? ServiceId { get; set; }

        public string Title { get; set; } = null!;
        public string ActionTypeType { get; set; } = null!;
        public List<TempLateDocDTO> TempLateDoc { get; set; } = new();
        public ActionTypeDTO? ActionType { get; set; }
		public List<FormGroupDTO> FormGroups { get; set; } = new();

		public IList<AssignUserDTO?>? AssignUsers { get; set; }
        //public string? ActionType { get; set; } 
        public bool IsOtherAttachment { get; set; } 
        public bool IsOtherAttachmentRequired { get; set; } 
        public string? AttachmentLabel { get; set; } 
        public bool IsRemark { get; set; } 
        public bool IsRemarkRequired { get; set; } 
        public string? RemarkLabel { get; set; }

        public List<AttachementDTO> SchAttachments { get; set; } = new();
        public List<string> SchAttachmentIds { get; set; } = new();

    }
}
