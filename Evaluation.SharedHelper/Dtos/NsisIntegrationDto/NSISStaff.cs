
namespace Evaluation.SharedHelper.Dtos.NsisIntegrationDto;

public class NSISStaff
{
    public string QId { get; set; }
    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public List<string?> Phones { get; set; }
    public List<string?> Sms { get; set; }
    public List<string?> Address { get; set; }
    public string Role { get; set; }
}