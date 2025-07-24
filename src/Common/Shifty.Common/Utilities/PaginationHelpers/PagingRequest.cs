using Shifty.Common.General.Enums.Searches;

namespace Shifty.Common.Utilities.PaginationHelpers;

public record PagingRequest(
    int PageNumber,
    int PageSize,
    SortDirection OrderBy = SortDirection.Latest
);