using System.Reflection.PortableExecutable;
using Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Calendars;
using Evaluation.DAL.Entities.Planing;

namespace Evaluation.DAL.Entities.FormsModules;

public class ScopeAcademicYear : BaseEntities
{
    public Guid DepartmentId { get; set; }
    public Department? Department { get; set; }
    public Guid ScopeId { get; set; }
    public Scope? Scope { get; set; }
    public Guid ParentId { get; set; }
    public Scope? Parent { get; set; }
    public Guid AcademicYearId { get; set; }
    public AcademicYear AcademicYear { get; set; } = null!;
}
