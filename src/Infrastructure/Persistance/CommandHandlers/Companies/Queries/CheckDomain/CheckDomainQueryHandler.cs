using MediatR;
using Shifty.Application.Companies.Queries.GetCompanyInfo;
using Shifty.Common.Exceptions;
using Shifty.Domain.Interfaces.Companies;
using System.Threading;
using System.Threading.Tasks;

namespace Shifty.Persistence.CommandHandlers.Companies.Queries.CheckDomain
{
    public class CheckDomainQueryHandler(ICompanyRepository companyRepository) : IRequestHandler<CheckDomainQuery , CheckDomainResponse>
    {
        public async Task<CheckDomainResponse> Handle(CheckDomainQuery request , CancellationToken cancellationToken)
        {
            if (request is null)
                throw new InvalidNullInputException(nameof(request));

            var validation = await companyRepository.ValidateDomain(request.Domain , cancellationToken);

            return new CheckDomainResponse(validation.IsValid , validation.message);
        }
    }
}


