using Evaluation.DAL.Entities.AdminPanel;

namespace Evaluation.DAL.Entities;

public class FormScope : BaseEntities
{
    public Guid FormId { get; set; }
    public Guid ScopeId { get; set; }
    public int Wegiht { get; set; }
}
