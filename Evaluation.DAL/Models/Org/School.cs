using Evaluation.DAL.Models.Audit;

namespace Evaluation.DAL.Models.Org;

public class School : OrgTree , IAuditLogEntity
{
    public Guid TypeId { get; set; }//بنين وبنات
    public SchoolType SchoolType { get; set; } 
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

    public virtual ICollection<SchoolLevel>? SchoolLevel { get; set; }
    public virtual ICollection<SchoolClass>? Classes { get; set; }
    
}
