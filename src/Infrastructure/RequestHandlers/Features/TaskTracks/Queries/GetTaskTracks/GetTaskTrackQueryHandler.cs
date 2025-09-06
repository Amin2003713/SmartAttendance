using Mapster;
using Shifty.Application.Features.TaskTrack.Queries.GetTackTracks;
using Shifty.Application.Features.TaskTrack.Requests.Queries.GetTaskTracks;
using Shifty.Application.Interfaces.Base.EventInterface;
using Shifty.Common.Exceptions;
using Shifty.Common.Utilities.DynamicTableHelper;
using Shifty.Common.Utilities.PaginationHelpers;
using Shifty.Domain.TaskTracks.Aggregate;
using Shifty.Persistence.Services.Identities;

namespace Shifty.RequestHandlers.Features.TaskTracks.Queries.GetTaskTracks;

public class GetTaskTrackQueryHandler(
    IdentityService identityService,
    IEventReader<TaskTrack, Guid> eventReader,
    ITableTranslatorService<GetTaskTrackQueryHandler> tableTranslatorService,
    ILogger<GetTaskTrackQueryHandler> logger,
    IStringLocalizer<GetTaskTrackQueryHandler> localizer
)
    : IRequestHandler<GetTaskTrackQuery, PagedResult<GetTaskTrackResponse>>
{
    public async Task<PagedResult<GetTaskTrackResponse>> Handle(
        GetTaskTrackQuery request,
        CancellationToken cancellationToken)
    {
        var userId = identityService.GetUserId<Guid>();
        logger.LogInformation("User {UserId} is requesting TaskTracks , Page {PageNumber}",
            userId,
            request.PageNumber);

        try
        {
            logger.LogInformation(
                "Fetching TaskTracks - Page {PageNumber}, Size {PageSize}",
                request.PageNumber,
                request.PageSize);

            var count = 0;
            var pageCount = 0;

            var events = await eventReader.LoadHybridAsync(
                null!,
                query => query.GetPaged(request.PageNumber, request.PageSize, out count, out pageCount),
                cancellationToken: cancellationToken);

            var responses = events.Adapt<List<GetTaskTrackResponse>>();

            logger.LogInformation(
                "Retrieved {Count} Missions(s)  on page {PageNumber}",
                count,
                request.PageNumber);

            return new PagedResult<GetTaskTrackResponse>
            {
                Headers = tableTranslatorService.GetColumnInfos<GetTaskTrackResponse>(),
                Results = responses,
                PageSize = request.PageSize,
                CurrentPage = request.PageNumber,
                PageCount = pageCount,
                RowCount = count
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Error occurred while retrieving TaskTracks ");

            throw ShiftyException.InternalServerError(
                localizer["An error occurred while retrieving contractor data."].Value);
        }
    }
}