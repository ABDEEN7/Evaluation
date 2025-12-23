

namespace Evaluation.SharedHelper.Models.Admin;
public class SchoolsDTO : EntityBaseDTO
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public Guid TypeId { get; set; }
    public string Type { get; set; } = null!;

    public Guid OrgClassId { get; set; }
    public string OrgClass { get; set; } = null!;
    public DateOnly EstablishmentDate { get; set; }
    public string? ManagerQID { get; set; }
    public string? ManageEmail { get; set; }
    public string? OrgEmail { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Mobile { get; set; }
    public string Code { get; set; } = null!;
    public string? Region { get; set; }
    public bool IsAccredited { get; set; }
    public DateOnly? AcceditedDate { get; set; }
    public bool SupportIdentity { get; set; }
    public DateOnly? SupportIdentityDate { get; set; }
}
