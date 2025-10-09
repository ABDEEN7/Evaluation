using Evaluation.DAL.Entities.AdminPanel;
using Evaluation.DAL.Entities.OrganizationTrees;

namespace Evaluation.DAL.Entities.Planing;

public class ChangeRequestDetail : BaseEntities
{
    public int ChangeRequestId { get; set; }
    public ChangeRequest ChangeRequest { get; set; } = new();
    public int SchoolId { get; set; }
    public School? School { get; set; }
    //public DateTime ScheduledDate { get; set; }
    public  string Reason { get; set; } = null!;
    public string? EvidenceDocument { get; set; } //URL or file path to evidence document
}