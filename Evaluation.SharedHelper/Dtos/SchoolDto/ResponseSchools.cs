namespace Evaluation.SharedHelper.Dtos.SchoolDto;

public class ResponseSchools
{
    public Guid? Id { get; set; }
    public string NameAr { get; set; } = null!;
	public string NameEn { get; set; } = null!;
	public Guid? OrgParentId { get; set; }
	public string? HrCode { get; set; }
	public string? NSISCode { get; set; }
	public Guid OrgTypeId { get; set; }
	public string? OrgTypeName { get; set; }
	public Guid OrgClassId { get; set; }
	public string? OrgClassName { get; set; }
	public bool IsAccredited { get; set; }
	public DateOnly? AcceditedDate { get; set; }
	public bool SupportIdentity { get; set; }
	public DateOnly SupportIdentityDate { get; set; }

	public Guid TypeId { get; set; }
	public string? SchoolTypeName { get; set; }
	public DateOnly EstablishmentDate { get; set; }
	public string? ManagerQID { get; set; }
	public string? ManageEmail { get; set; }
	public string? OrgEmail { get; set; }
	public string? Address { get; set; }
	public string? Phone { get; set; }
	public string? Mobile { get; set; }
	public string Code { get; set; } = null!;
	public string? Region { get; set; }

	public List<string> Levels { get; set; } = new();

	public DateTime? LastEvaluationDate { get; set; }
	public string? Rating { get; set; }
	public int AcademicYear { get; set; }
}
