namespace SmartAttendance.Application.Features.Plans.Commands;

public sealed record CancelEnrollmentCommand(
    Guid PlanId,
    Guid StudentId
) : IRequest;