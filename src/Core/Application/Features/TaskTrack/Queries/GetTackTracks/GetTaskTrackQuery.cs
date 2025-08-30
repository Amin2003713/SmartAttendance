using Shifty.Common.General.Enums.Searches;
using Shifty.Common.Utilities.PaginationHelpers;
using TaskTracker.Application.TaskTrack.Requests.Queries.GetTaskTracks;

namespace Shifty.Application.Features.TaskTrack.Queries.GetTackTracks;

public record GetTaskTrackQuery(
    int PageNumber = 1,
    int PageSize = int.MaxValue,
    SortDirection OrderBy = SortDirection.Latest
) : PagingRequest(PageNumber, PageSize, OrderBy),
    IRequest<PagedResult<GetTaskTrackResponse>>;