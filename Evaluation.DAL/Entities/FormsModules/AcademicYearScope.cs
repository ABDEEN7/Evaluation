using  Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Calendars;

namespace Evaluation.DAL.Entities.FormsModules;

public class AcademicYearScope : EntityBase
{
    public Guid? ScopeId { get; set; }
    public Guid? ParentId { get; set; }
    public Guid? AcademicYearId { get; set; }
    public Scope? Scope { get; set; }
    public Scope? Parent { get; set; }
    public AcademicYear? AcademicYear { get; set; }

}
