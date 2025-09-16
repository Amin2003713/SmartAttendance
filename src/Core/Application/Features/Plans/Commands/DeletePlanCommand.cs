namespace SmartAttendance.Application.Features.Plans.Commands;

public sealed record DeletePlanCommand(
    Guid PlanId
) : IRequest;