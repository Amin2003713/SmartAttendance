namespace SmartAttendance.Application.Features.Plans.Commands.ReshcedualPlan;

public class ReschedulePlanRequest :
    IRequest
{
    public Guid Id { get; init; }
    public DateTime Start { get; init; }
    public DateTime End { get; init; }
}