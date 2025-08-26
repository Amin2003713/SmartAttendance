namespace Shifty.Application.Features.Stations.Commands.Delete;

public record DeleteStationCommand(Guid Id) : IRequest;