using SmartAttendance.Application.Base.Universities.Commands.AddRequest;
using SmartAttendance.Application.Interfaces.Tenants.Companies;

namespace SmartAttendance.RequestHandlers.Base.Universites.Commands.AddRequest;

public class AddRequestCommandHandler(
    IUniversityRepository UniversityRepository
) : IRequestHandler<AddRequestCommand>
{
    public async Task Handle(AddRequestCommand request, CancellationToken cancellationToken)
    {
        await UniversityRepository.AddRequest(request, cancellationToken);
    }
}