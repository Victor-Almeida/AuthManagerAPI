using Microsoft.EntityFrameworkCore;
using AuthManager.Domain.Primitives;

namespace AuthManager.Domain.Extensions;

public static class PaginatedListExtensions
{
    private static int CalculateRecordsToSkip(int page, int pageSize) => (page - 1) * pageSize;

    public static PaginatedList<T> ToPaginatedList<T>(this IQueryable<T> query, int page = 1, int pageSize = 10)
    {
        var records = query
            .Skip(CalculateRecordsToSkip(page, pageSize))
            .Take(pageSize)
            .ToList();

        var totalRecords = query.Count();

        return new PaginatedList<T>(
            records,
            page,
            pageSize,
            totalRecords);
    }

    public static async Task<PaginatedList<T>> ToPaginatedListAsync<T>(
        this IQueryable<T> query, 
        int page = 1, 
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var records = await query
            .Skip(CalculateRecordsToSkip(page, pageSize))
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var totalRecords = await query.CountAsync(cancellationToken);

        return new PaginatedList<T>(
            records,
            page,
            pageSize,
            totalRecords);
    }
}
