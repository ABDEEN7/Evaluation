using Evaluation.DAL.Entities.Base;

namespace Evaluation.DAL.Entities.FormsModules;

public class AcademicYearScope : BaseEntities
{
    public Guid ScopeId { get; set; }
    public Guid ParentId { get; set; }
    public Scope? Scope { get; set; }
    public Scope? Parent { get; set; }
}
