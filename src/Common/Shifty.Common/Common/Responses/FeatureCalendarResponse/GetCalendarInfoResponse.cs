namespace Shifty.Common.Common.Responses.FeatureCalendarResponse;

public record GetCalendarInfoResponse(
    string ItemName,
    string ItemNamePersian,
    int UnVerified,
    int Total,
    int Rejected,
    int Verified
);