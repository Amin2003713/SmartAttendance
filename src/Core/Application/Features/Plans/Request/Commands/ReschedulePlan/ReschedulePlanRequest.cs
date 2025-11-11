namespace SmartAttendance.Application.Features.Plans.Request.Commands.ReschedulePlan;

public class ReschedulePlanRequest 
{
    public Guid Id { get; init; }
    public DateTime Start { get; init; }
    public DateTime End { get; init; }
}