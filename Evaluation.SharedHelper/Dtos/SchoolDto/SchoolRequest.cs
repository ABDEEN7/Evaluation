using Evaluation.SharedHelper.Enums;

namespace Evaluation.SharedHelper.Dtos.SchoolDto;

public class SchoolRequest : PaginatedQuery
{
    public Guid StatusId { get; set; }
}
