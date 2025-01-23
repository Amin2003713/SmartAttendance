using MediatR;
using Shifty.Application.Companies.Queries.CheckDomain;
using Shifty.Common.Exceptions;
using Shifty.Domain.Interfaces.Companies;
using Shifty.Resources.Messages;
using System.Threading;
using System.Threading.Tasks;

namespace Shifty.Persistence.CommandHandlers.Companies.Queries.CheckDomain
{
    public class CheckDomainQueryHandler(ICompanyRepository companyRepository , CompanyMessages messages) : IRequestHandler<CheckDomainQuery , CheckDomainResponse>
    {
        public async Task<CheckDomainResponse> Handle(CheckDomainQuery request , CancellationToken cancellationToken)
        {
            if (request is null)
                throw new InvalidNullInputException(nameof(request));

            var validation = await companyRepository.ValidateDomain(request.Domain , cancellationToken);

            return new CheckDomainResponse(validation , (validation ? messages.Tenant_Is_Not_Valid() : messages.Tenant_Is_Valid()));
        }
    }
}