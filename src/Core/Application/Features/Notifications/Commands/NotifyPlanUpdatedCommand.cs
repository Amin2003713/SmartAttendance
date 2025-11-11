using SmartAttendance.Common.Common.Requests;

namespace SmartAttendance.Application.Features.Notifications.Commands;

public class NotifyPlanUpdatedCommand : IRequest
{
    public Guid PlanId { get; init; }
    public required List<Guid> ToUser { get; init; }
    public bool IsTimeChanged { get; init; }
    public bool IsLocationChanged { get; init; }
    public DateTime NewStart { get; init; }
    public DateTime NewEnd { get; init; }
    public Location NewLocation { get; init; } = default!;
    public string NewAddress { get; init; } = default!;
}