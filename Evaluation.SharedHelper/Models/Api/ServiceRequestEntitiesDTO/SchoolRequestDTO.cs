using Evaluation.SharedHelper.Dtos.SchoolDto;

namespace Evaluation.SharedHelper.Models.Api.ServiceRequestEntitiesDTO;

public class SchoolRequestDTO
{
    public SchoolRequestDTO()
    {
        Data = new List<ResponseSchools>();
    }
    public List<ResponseSchools> Data { get; set; }

    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalDataCount { get; set; }

    public bool IsRemainingData { get; set; }
}