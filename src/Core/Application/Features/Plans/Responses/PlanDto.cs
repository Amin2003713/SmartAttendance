namespace SmartAttendance.Application.Features.Plans.Responses;

// DTO نمایش اطلاعات طرح
public sealed class PlanDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime StartsAtUtc { get; init; }
    public DateTime EndsAtUtc { get; init; }
    public int Capacity { get; init; }
    public int Reserved { get; init; }
}