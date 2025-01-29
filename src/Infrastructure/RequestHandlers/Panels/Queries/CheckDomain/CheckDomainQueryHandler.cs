using CancellationToken = System.Threading.CancellationToken;

using MediatR;
using Shifty.Application.Panel.Companies.Queries.CheckDomain;
using Shifty.Common.Exceptions;
using Shifty.Domain.Interfaces.Tenants.Companies;
using Shifty.Resources.Messages;

namespace Shifty.RequestHandlers.Panels.Queries.CheckDomain
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