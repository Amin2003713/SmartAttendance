using SmartAttendance.Application.Base.Companies.Commands.AddRequest;
using SmartAttendance.Application.Interfaces.Tenants.Companies;

namespace SmartAttendance.RequestHandlers.Base.Companies.Commands.AddRequest;

public class AddRequestCommandHandler(
    ICompanyRepository companyRepository
) : IRequestHandler<AddRequestCommand>
{
    public async Task Handle(AddRequestCommand request, CancellationToken cancellationToken)
    {
        await companyRepository.AddRequest(request, cancellationToken);
    }
}