using SmartAttendance.Application.Features.Plans.Commands.Create;

namespace SmartAttendance.RequestHandlers.Features.Plans.Commands.Create;

public class CreatePlanCommandHandler : IRequestHandler<CreatePlanCommand>
{
    public Task Handle(CreatePlanCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}