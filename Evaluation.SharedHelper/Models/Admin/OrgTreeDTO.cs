

namespace Evaluation.SharedHelper.Models.Admin;
public class OrgTreeDTO : EntityBaseDTO
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public Guid? OrgParentId { get; set; }
    public string? OrgParent { get; set; }
    public string? HrCode { get; set; }
    public string? NSISCode { get; set; }
    public Guid OrgTypeId { get; set; }
    public string? OrgType { get; set; }
    public Guid OrgClassId { get; set; }
    public string? OrgClass { get; set; }
}
