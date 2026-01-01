

namespace Evaluation.SharedHelper.Models.Admin;
public class EmployeesDTO : EntityBaseDTO
{
    public string NameAr { get; set; } = null!;
    public string NameEn { get; set; } = null!;
    public string EmployeeNo { get; set; } = null!;
    public Guid OrgClassId { get; set; }
    public string OrgClass { get; set; } = null!;
    public Guid UserGenderId { get; set; }
    public string? UserGender { get; set; }
    public DateOnly BirthDate { get; set; }
    public string NationalityCode { get; set; } = null!;
    public DateOnly JoinDate { get; set; }
    public Guid JobTitleId { get; set; }
    public string JobTitle { get; set; } = null!;
    public string? Email { get; set; }
    public string QID { get; set; } = null!;
    public bool IsOrgManager { get; set; }
}
