
namespace Evaluation.SharedHelper.Dtos.NsisIntegrationDto;

public class NSISSchool
{
    public string? Id { get; set; }
    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public string? Status { get; set; }
    public string? DateLastModified { get; set; }
    public List<NSISTeacher>? Teachers { get; set; }
    public List<NSISStaff>? Staff { get; set; }
    public List<NSISClass>? Classes { get; set; }
}