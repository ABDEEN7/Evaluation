using System.Diagnostics;
using Evaluation.DAL.Entities.AdminPanel;

namespace Evaluation.DAL.Entities;

public class EvaluationFieldUsers : BaseEntities
{
    public Guid UserId { get; set; }
    public Guid TeamId { get; set; }
    public Guid EvaluationFieldId { get; set; }
}
