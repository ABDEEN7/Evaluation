using System.Diagnostics;
using Evaluation.DAL.Entities.AdminPanel;
using Evaluation.DAL.Entities.Authentication;
using Evaluation.DAL.Entities.Planing;

namespace Evaluation.DAL.Entities;

public class EvaluationFieldUsers : BaseEntities
{
    public Guid UserId { get; set; }
    public Guid TeamId { get; set; }
    public Guid EvaluationFieldId { get; set; }
    public User? User { get; set; }
    public Team? Team { get; set; }
    public EvaluationField? EvaluationField { get; set; }
}
