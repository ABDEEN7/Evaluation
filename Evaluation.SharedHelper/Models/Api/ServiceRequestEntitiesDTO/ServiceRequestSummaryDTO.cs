namespace Scholarship.SharedHelper.Models.Api.ServiceRequestEntitiesDTO;

public class ServiceRequestSummaryDTO
{
    public Guid RequestId { get; set; }
    public string RequestNumber { get; set; }
    public string ServiceName { get; set; }
    public string QID { get; set; }
    public string ServiceRequestStatus { get; set; }
}