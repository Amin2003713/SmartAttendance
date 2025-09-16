
namespace SmartAttendance.Common.Utilities.PaginationHelpers;

public record PagingRequest(
    int           PageNumber,
    int           PageSize);