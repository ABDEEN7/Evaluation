namespace Evaluation.SharedHelper.Dtos.NsisIntegrationDto;

public class Teacher : NSISBaseDto
{
    public string? Username { get; set; }
    public string? Identifier { get; set; }
    public string? GivenName { get; set; }
    public string? FamilyName { get; set; }
    public string? MiddleName { get; set; }
    public string? Phone { get; set; }
    public string? Sms { get; set; }
    public string? Role { get; set; }
    //public string? Agents { get; set; }
    public string? Email { get; set; }
    public TeacherMetaData? MetaData { get; set; }
}
