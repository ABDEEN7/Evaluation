using Evaluation.DAL.Entities.Audit;
using  Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Calendars;
using Evaluation.DAL.Entities.DepartementEntites;
using Evaluation.DAL.Entities.Planing;
using System.Reflection.PortableExecutable;

namespace Evaluation.DAL.Entities.FormsModules;

public class ScopeAcademicYear : EntityBase, IAuditLogEntity
{
    public Guid DepartmentId { get; set; }
    public Department? Department { get; set; }
    public Guid ScopeId { get; set; }
    public Scope? Scope { get; set; }
    public Guid AcademicYearId { get; set; }
    public AcademicYear AcademicYear { get; set; } = null!;
}
