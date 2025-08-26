using GetCalendarInfoResponse = Shifty.Common.Common.Responses.FeatureCalendarResponse.GetCalendarInfoResponse;

namespace Shifty.Application.Base.Calendars.Queries.GetCalendarInfo;

public record GetCalendarInfoQuery(
    
    DateTime Date
) : IRequest<List<GetCalendarInfoResponse>>;