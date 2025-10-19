namespace Evaluation.DAL.Entities.Planing.TeamsModule;

public class Team //I chaneg the name of DepEvalTeams to team
{
    public int Id { get; set; }
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public Guid DepartmentId { get; set; }
}