namespace SmartAttendance.Application.Features.Missions.Commands.Delete;

public record DeleteMissionCommand(
    Guid AggregateId
) : IRequest;