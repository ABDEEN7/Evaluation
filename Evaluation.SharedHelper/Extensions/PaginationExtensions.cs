using Evaluation.SharedHelper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace Evaluation.SharedHelper.Extensions;

public static class PaginationExtensions
{
    public static async Task<PaginatedResult<T>> GetPaginatedResult<T>(this IQueryable<T> items, int pageNumber, int pageSize)
    {
        if (pageNumber < 1) throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be greater than 0.");
        if (pageSize < 1) throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than 0.");

        var totalCount = await items.CountAsync();
        var pagedItems = items.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        return new PaginatedResult<T>(pagedItems, totalCount, pageNumber, pageSize);
    }
}
