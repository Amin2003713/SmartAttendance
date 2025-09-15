using SmartAttendance.Common.General.Enums.Searches;

namespace SmartAttendance.Common.Utilities.PaginationHelpers;

public record PagingRequest(
    int           PageNumber,
    int           PageSize,
    SortDirection OrderBy = SortDirection.Latest
);