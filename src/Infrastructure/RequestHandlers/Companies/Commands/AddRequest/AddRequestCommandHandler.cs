using Shifty.Application.Companies.Commands.AddRequest;
using Shifty.Application.Interfaces.Tenants.Companies;

namespace Shifty.RequestHandlers.Companies.Commands.AddRequest;

public class AddRequestCommandHandler(
    ICompanyRepository companyRepository
) : IRequestHandler<AddRequestCommand>
{
    public async Task Handle(AddRequestCommand request, CancellationToken cancellationToken)
    {
        await companyRepository.AddRequest(request, cancellationToken);
    }
}