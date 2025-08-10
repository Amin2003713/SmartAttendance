using Shifty.Application.Stations.Commands.Create;

namespace Shifty.RequestHandlers.Stations.Commands.Create;

public class CreateStationCommandHandler : IRequestHandler<CreateStationCommand>
{
    public Task Handle(CreateStationCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}