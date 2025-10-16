using Microsoft.EntityFrameworkCore;
using Evaluation.DAL.Entities.Audit;
using Evaluation.DAL.Entities.Base;
using Evaluation.DAL.Entities.FormBuilder;
using Evaluation.DAL.Entities.ServicesEntities;


namespace Evaluation.DAL.Entities.ActionEntities
{
    [Index(nameof(BackendName), IsUnique = true)]
    public class ServiceAction : BaseEntities, IAuditLogEntity
    {
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public string BackendName { get; set; } = null!;
        public bool IsConfirmationAction { get; set; }
        public string? ConfirmationTitleAr { get; set; }
        public string? ConfirmationTitleEn { get; set; }
        public string? ConfirmationBodyAr { get; set; }
        public string? ConfirmationBodyEn { get; set; }
        public Guid ActionTypeId { get; set; }
        public ActionType? ActionType { get; set; }
        public Guid ServiceId { get; set; }
        public Service? Service { get; set; }
        public bool IsInitialAction { get; set; }
        public bool IsAutoAssign { get; set; }
        public bool AllowDraft { get; set; }

        public Guid? NewStatusId { get; set; } // need To Add Relation Later
        
        public virtual ICollection<ActionField>? ActionStepsFields { get; } = new List<ActionField>();
        public virtual ICollection<ActionStatusConfiguration>? NextStatusConfiguration { get; } = new List<ActionStatusConfiguration>();
        public virtual ICollection<ActionTemplateDoc>? ActionTemplateDocs { get; } = new List<ActionTemplateDoc>();      
        public virtual ICollection<ActionShowLogPartyType>? ActionShowLogPartyTypes { get; } = new List<ActionShowLogPartyType>();

    }
}
