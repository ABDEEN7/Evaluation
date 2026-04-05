namespace Evaluation.SharedHelper.Dtos.NsisIntegrationDto;

public class EnrollmentDto : NSISBaseDto
{
    public Guid SchoolId { get; set; }
    public Guid ClassId { get; set; }
    public Guid Userid { get; set; }
    public string Usertype { get; set; }
    public string Role { get; set; }
}
