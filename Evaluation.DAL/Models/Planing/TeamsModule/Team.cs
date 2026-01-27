using Evaluation.DAL.Models.Audit;
using Evaluation.DAL.Models.BaseModule;
using Evaluation.DAL.Models.DepartementEntites;
using System.ComponentModel;

namespace Evaluation.DAL.Models.Planing.TeamsModule;

public class Team : EntityBase, IAuditLogEntity
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public Guid DepartmentId { get; set; }
    public Department? Department { get; set; }
    //public ICollection<UserTeam>? UserTeams { get; set; }
}