using Mapster;
using Shifty.Application.Calendars.Queries.GetCalendarInfo;
using Shifty.Common.Common.Responses.FeatureCalendarResponse;
using Shifty.Common.Exceptions;
using Shifty.Common.General;

using Shifty.Common.Messaging.Contracts.PRIMA.Items.Queries.GetPrimaCalendarInfoForDay;
using Shifty.Common.Messaging.Contracts.Tenants.Projects.ProjectUsers.GetProjectUserAccess;
using Shifty.Persistence.Services.Identities;

namespace Shifty.RequestHandlers.Calendars.Queries.GetCalendarInfoForDay;

public class GetCalendarInfoForDayQueryHandler(
    IdentityService service,
    IMediator mediator,
    ILogger<GetCalendarInfoForDayQueryHandler> logger
)
    : IRequestHandler<GetCalendarInfoQuery, List<GetCalendarInfoResponse>>
{
    public async Task<List<GetCalendarInfoResponse>> Handle(
        GetCalendarInfoQuery request,
        CancellationToken cancellationToken)
    {
        var userId = service.GetUserId<Guid>();

        logger.LogInformation("User {UserId} is requesting calendar info for ProjectId: {ProjectId} on Date: {Date}",
            userId,
            request.ProjectId,
            request.Date.ToShortDateString());

        try
        {
            var access =
                await mediator.Send(new GetProjectUserQuery(request.ProjectId, userId, ApplicationConstant.ServiceName),
                    cancellationToken);


            if (access == null)
                throw IpaException.Forbidden("Access denied");

            var result =
                await broker.RequestAsync<GetPrimaCalendarInfoForDayBrokerResponse, GetPrimaCalendarInfoForDayBroker>(
                    new GetPrimaCalendarInfoForDayBroker
                    {
                        Access = access.Adapt<GetProjectUserAccessBrokerResponse>(), Date = request.Date, ProjectId = request.ProjectId
                    },
                    cancellationToken);


            return result.Result;
        }
        catch (NotFoundException)
        {
            return [];
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Unexpected error while generating calendar info for ProjectId {ProjectId} on {Date}",
                request.ProjectId,
                request.Date);

            throw;
        }
    }
}