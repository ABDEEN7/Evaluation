using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.Calendars;

namespace Evaluation.DAL.Models.FormsModules;

public class AcademicYearScope : EntityBase, IAuditLogEntity
{
    public Guid? ScopeId { get; set; }
    public Guid? ParentId { get; set; }
    public Guid? AcademicYearId { get; set; }
    public Scope? Scope { get; set; }
    public Scope? Parent { get; set; }
    public AcademicYear? AcademicYear { get; set; }

}
