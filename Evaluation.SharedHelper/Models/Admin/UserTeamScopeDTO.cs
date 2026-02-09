namespace Evaluation.SharedHelper.Models.Admin;

public class UserTeamScopeDTO : EntityBaseDTO
{
    public Guid TeamId { get; set; }
    public Guid UserId { get; set; }
    public string? User { get; set; }
    public Guid[]? ScopeId { get; set; }
    public string[]? Scope { get; set; }

}
public class UserTeamScopeDetailDTO 
{
    public Guid Id { get; set; }
    public Guid[]? Scope { get; set; }
    public Guid? User { get; set; }
    public bool IsActive { get; set; }
    

}




