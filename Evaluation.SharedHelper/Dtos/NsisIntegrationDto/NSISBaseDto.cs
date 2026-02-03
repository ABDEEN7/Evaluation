
namespace Evaluation.SharedHelper.Dtos.NsisIntegrationDto;

public abstract class NSISBaseDto
{
    public Guid sourcedId { get; set; }
    public string? Status { get; set; }
    public string? DateLastModified { get; set; }
}
