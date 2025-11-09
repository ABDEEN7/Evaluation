using  Evaluation.DAL.Entities.BaseModule;
using Evaluation.DAL.Entities.Org;

namespace Evaluation.DAL.Entities.Planing;

public class ChangeRequestDetail : EntityBase
{
    public Guid ChangeRequestId { get; set; }
    public ChangeRequest ChangeRequest { get; set; } = new();
    public Guid SchoolId { get; set; }
    public School? School { get; set; }
    //public DateTime ScheduledDate { get; set; }
    public  string Reason { get; set; } = null!;
    public string? EvidenceDocument { get; set; } //URL or file path to evidence document
}