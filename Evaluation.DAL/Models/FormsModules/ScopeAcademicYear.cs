using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.Calendars;
using Evaluation.DAL.Models.DepartementEntites;
using System.Reflection.PortableExecutable;

namespace Evaluation.DAL.Models.FormsModules;

public class ScopeAcademicYear : EntityBase, IAuditLogEntity
{
    public Guid DepartmentId { get; set; }
    public Department? Department { get; set; }
    public Guid ScopeId { get; set; }
    public Scope? Scope { get; set; }
    public Guid AcademicYearId { get; set; }
    public AcademicYear AcademicYear { get; set; } = null!;
}
