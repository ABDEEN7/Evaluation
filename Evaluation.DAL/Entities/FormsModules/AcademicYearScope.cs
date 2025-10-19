using  Evaluation.DAL.Entities.BaseModule;

namespace Evaluation.DAL.Entities.FormsModules;

public class AcademicYearScope : EntityBase
{
    public Guid ScopeId { get; set; }
    public Guid ParentId { get; set; }
    public Scope? Scope { get; set; }
    public Scope? Parent { get; set; }
}
