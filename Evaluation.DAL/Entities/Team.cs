namespace Evaluation.DAL.Entities;

public class Team //I chaneg the name of DepEvalTeams to team
{
    public int Id { get; set; }
    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public Guid DepartmentId { get; set; }
}