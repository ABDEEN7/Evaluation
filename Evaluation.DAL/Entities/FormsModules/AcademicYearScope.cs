using Evaluation.DAL.Entities.AdminPanel;
using Evaluation.DAL.Entities.Planing;

namespace Evaluation.DAL.Entities.FormsModules;

public class AcademicYearScope : BaseEntities
{
    public Guid DepartmentId { get; set; }
    public Department? Department { get; set; }
    public Guid ScopeId { get; set; }
    public Guid ParentId { get; set; }
    public Scope? Scope { get; set; }
    public Scope? Parent { get; set; }
}
