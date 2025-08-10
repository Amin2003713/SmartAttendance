namespace Shifty.Application.Stations.Commands.Delete;

public record DeleteStationCommand(Guid Id) : IRequest;