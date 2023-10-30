namespace AuthManager.Application.Filters;

public record PaginationFilter(int Page = 1, int PageSize = 10);
