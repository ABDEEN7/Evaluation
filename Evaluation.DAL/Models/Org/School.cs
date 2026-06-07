using Evaluation.DAL.Models.Audit;

namespace Evaluation.DAL.Models.Org;

public class School : OrgTree , IAuditLogEntity
{
    public Guid TypeId { get; set; }//ثابت حكومي من  hr // 
    public SchoolType? SchoolType { get; set; } 
    public DateOnly EstablishmentDate { get; set; } // date now 
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
    public Guid? SchoolStatusId { get; set; }
    public SchoolStatus? SchoolStatus { get; set; }
    public DateOnly? CloseDate { get; set; }
    public Guid? SchoolGenderId { get; set; }
    public SchoolGender? SchoolGender { get; set; }
    public Guid? SchoolModelId { get; set; }
    public SchoolModel? SchoolModel { get; set; }
    public string? LATITUDE { get; set; }
    public string? LONGITUDE { get; set; }
    public string? URL { get; set; }
    public Guid? SchoolProgramId { get; set; }
    public SchoolProgram? SchoolProgram { get; set; }
    public int SchoolCapacity { get; set; } = 0;

    public Guid? SchoolEmpGenderId { get; set; }
    public SchoolGender? SchoolEmpGender { get; set; }

    public virtual ICollection<SchoolLevel>? SchoolLevel { get; set; }
    
}
