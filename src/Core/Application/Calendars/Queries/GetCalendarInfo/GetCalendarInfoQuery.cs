using GetCalendarInfoResponse = Shifty.Common.Common.Responses.FeatureCalendarResponse.GetCalendarInfoResponse;

namespace Shifty.Application.Calendars.Queries.GetCalendarInfo;

public record GetCalendarInfoQuery(
    Guid ProjectId,
    DateTime Date
) : IRequest<List<GetCalendarInfoResponse>>;