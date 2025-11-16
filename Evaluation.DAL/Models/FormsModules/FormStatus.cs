using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;

namespace Evaluation.DAL.Models.FormsModules;

public class FormStatus : EntityBase, IAuditLogEntity
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public string BackendName { get; set; } = null!;
}
