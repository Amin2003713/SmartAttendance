using SmartAttendance.Application.Features.TaskTrack.Requests.Queries.GetTaskTracks;
using SmartAttendance.Common.General.Enums.Searches;
using SmartAttendance.Common.Utilities.PaginationHelpers;

namespace SmartAttendance.Application.Features.TaskTrack.Queries.GetTackTracks;

public record GetTaskTrackQuery(
    int PageNumber = 1,
    int PageSize = int.MaxValue,
    SortDirection OrderBy = SortDirection.Latest
) : PagingRequest(PageNumber, PageSize, OrderBy),
    IRequest<PagedResult<GetTaskTrackResponse>>;