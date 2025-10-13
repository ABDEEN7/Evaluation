using Evaluation.DAL.Entities.BaseModule;

namespace Evaluation.DAL.Entities.FormsModules;

public class FormScope : BaseEntities
{
    public Guid FormId { get; set; }
    public Guid ScopeId { get; set; }
    public Form? Form { get; set; }
    public Scope? Scope { get; set; }
    public int Wegiht { get; set; }
}
