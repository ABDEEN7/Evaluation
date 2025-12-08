namespace Evaluation.SharedHelper.Models;

public record PaginatedResult<T>(List<T> Items, long TotalCount, int PageNumber = 1, int PageSize = 10, object? AdditionalProperties = null)
{
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

}
