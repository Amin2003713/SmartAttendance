using Shifty.Common.General.Enums;

namespace Shifty.Common.Common.Responses.ItemNamesBaseResponse;

public record DataReviewLog
{
    public Guid PerformedById { get; init; }

    public string PerformedByName { get; init; }
    public ReviewAction Action { get; init; }
    public string Comment { get; init; }

    public DateTime DataReviewedAt { get; init; } = DateTime.UtcNow;

    public UserType PerformedByLevel { get; init; }
}