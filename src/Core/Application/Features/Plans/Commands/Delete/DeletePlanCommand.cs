namespace SmartAttendance.Application.Features.Plans.Commands.Delete;

public record DeletePlanCommand(
    Guid Id
)  : IRequest;
