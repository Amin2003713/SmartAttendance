using Mapster;
using MediatR;
using Shifty.Application.Companies.Queries.GetCompanyInfo;
using Shifty.Application.Tenants.Command;
using Shifty.Common;
using Shifty.Domain.Interfaces.Companies;
using System.Threading;
using System.Threading.Tasks;

namespace Shifty.Persistence.CommandHandlers.Companies.Queries.GetCompanyInfo;

public class GetCompanyInfoQueryHandler(ICompanyRepository companyRepository)   : IRequestHandler<GetCompanyInfoQuery , GetCompanyInfoResponse>
{
    public async Task<GetCompanyInfoResponse> Handle(GetCompanyInfoQuery request , CancellationToken cancellationToken)
    {
        if(!await companyRepository.ExistsAsync(request.CompanyIdentifier , cancellationToken))
            throw new ShiftyException(ApiResultStatusCode.NotFound , "Company not found");

        var result = await companyRepository.GetByIdentifierAsync(request.CompanyIdentifier , cancellationToken);

        if (result == null)
            throw new ShiftyException(ApiResultStatusCode.NotFound , "Company not found");

        return result.Adapt<GetCompanyInfoResponse>();
    }
}