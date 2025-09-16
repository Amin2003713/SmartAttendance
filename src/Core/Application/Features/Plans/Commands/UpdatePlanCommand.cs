using SmartAttendance.Application.Features.Plans.Requests;

namespace SmartAttendance.Application.Features.Plans.Commands;

public sealed record UpdatePlanCommand(
    Guid PlanId,
    UpdatePlanRequest Request
) : IRequest;