namespace Evaluation.SharedHelper.Dtos.NsisIntegrationDto;

public class Teacher : NSISBaseDto
{
    public string? Username { get; set; }
    public string? Identifier { get; set; }//QID
    public string? GivenName { get; set; }
    public string? FamilyName { get; set; }
    public string? MiddleName { get; set; }
    public List<string?> Phone { get; set; }
    public List<string?> Sms { get; set; }
    public string? Role { get; set; }
    public string? Email { get; set; }

    public TeacherMetaData? MetaData { get; set; }
}
