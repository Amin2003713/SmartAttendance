namespace SmartAttendance.Application.Features.Notifications.Commands;

public class NotifyTimeHasChengedCommand : IRequest
{
    public Guid PlanId { get; init; }
    public List<Guid> ToUser { get; set; }
    public DateTime NewDateStart { get; set; }
    public DateTime NewDateEnd { get; set; }
}