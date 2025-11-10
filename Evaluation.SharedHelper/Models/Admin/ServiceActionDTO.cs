
namespace Evaluation.SharedHelper.Models.Admin
{
    public class ServiceActionDTO : EntityBaseDTO
    {
        public string NameAr { get; set; } = null!;

        public string NameEn { get; set; } = null!;

        //public string? BackendName { get; set; }
        public bool IsConfirmationAction { get; set; }
        public Guid ActionTypeId { get; set; }

        public Guid ServiceId { get; set; }

        public bool IsInitialAction { get; set; }
        public bool AllowDraft { get; set; }
        public string? ConfirmationBodyAr { get; set; }
        public string? ConfirmationBodyEn { get; set; }
        public string? ConfirmationTitleAr { get; set; }
        public string? ConfirmationTitleEn { get; set; }


        public List<Guid>? ActionPartyTypeList { get; set; }
        public List<Guid>? AssignActionPartyTypeList { get; set; }
        public List<Guid>? ActionShowLogPartyTypeList { get; set; }
        public List<Guid>? ActionTemplateDocList { get; set; }

    }
}

