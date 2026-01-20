namespace Evaluation.SharedHelper.Dtos.OrgDto;

public class OrgDetailsDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    ///School Info
    public string? TeachersCount { get; set; }
    public string? StudentsCount { get; set; }


    ///Employee Info
    public string? EmployeeNo { get; set; }
    public string? UserGender { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string? NationalityCode { get; set; }
    public string? JoinDate { get; set; }
    public string? JobTitle { get; set; }
    public string? Email { get; set; }
    public string QID { get; set; }


    ///Shared School & Orgnizations Info
    public DateOnly? EstablishmentDate { get; set; }
    public string? ManagerName { get; set; }
    public string? OrgEmail { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Mobile { get; set; }

}
