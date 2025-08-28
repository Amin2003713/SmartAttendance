using Shifty.Common.Common.Responses.FeatureCalendarResponse;

namespace Shifty.Application.Features.Calendars.Queries.GetCalendarInfo;

public record GetCalendarInfoQuery(
    DateTime Date
) : IRequest<List<GetCalendarInfoResponse>>;