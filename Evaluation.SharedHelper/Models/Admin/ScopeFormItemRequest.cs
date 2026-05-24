namespace Evaluation.SharedHelper.Models.Admin;

public class ScopeFormItemRequest
{
    public int Page { get; set; } = 1;
    public Guid? DepartmentId { get; set; }
}
