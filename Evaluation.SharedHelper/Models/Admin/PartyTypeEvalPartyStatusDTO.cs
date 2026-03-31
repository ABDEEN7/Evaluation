namespace Evaluation.SharedHelper.Models.Admin;

public class PartyTypeEvalPartyStatusDTO : EntityBaseDTO
{
    public Guid EvaluationPartyId { get; set; }
    public Guid PartyTypeId { get; set; }
    public string? PartyType { get; set; }
    public Guid[]? ServiceStatusId { get; set; }
    public string[]? ServiceStatus { get; set; }

}
public class PartyTypeEvalPartyStatusDetailDTO
{
    public Guid Id { get; set; }
    public Guid[]? ServiceStatus { get; set; }
    public Guid? PartyType { get; set; }
    public bool IsActive { get; set; }
    

}




