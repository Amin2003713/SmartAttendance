namespace SmartAttendance.Application.Features.Notifications.Commands;

public class NotifyPlanDeletedCommand : IRequest
{
    public Guid PlanId { get; init; }
    public required List<Guid> ToUser { get; init; }
}