namespace SmartAttendance.Application.Features.Plans.Requests;

// درخواست به‌روزرسانی اطلاعات طرح
public sealed class UpdatePlanRequest
{
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime StartsAtUtc { get; init; }
    public DateTime EndsAtUtc { get; init; }
    public int Capacity { get; init; }
}