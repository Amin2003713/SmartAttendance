namespace SmartAttendance.Application.Features.Stations.Commands.Delete;

public record DeleteStationCommand(
    Guid Id
) : IRequest;