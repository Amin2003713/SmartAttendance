using Mapster;
using MediatR;
using Shifty.Application.Companies.Queries.GetCompanyInfo;
using Shifty.Common.Exceptions;
using Shifty.Domain.Interfaces.Tenants.Companies;
using Shifty.Resources.Messages;

namespace Shifty.RequestHandlers.Companies.Queries.GetCompanyInfo
{
    public class GetCompanyInfoQueryHandler(ICompanyRepository companyRepository , CompanyMessages messages) : IRequestHandler<GetCompanyInfoQuery , GetCompanyInfoResponse>
    {
        public async Task<GetCompanyInfoResponse> Handle(GetCompanyInfoQuery request , CancellationToken cancellationToken)
        {
            if (!await companyRepository.IdentifierExistsAsync(request.Domain , cancellationToken))
                throw  ShiftyException.NotFound(messages.Company_Not_Found());

            var result = await companyRepository.GetByIdentifierAsync(request.Domain , cancellationToken);

            if (result == null)
                throw ShiftyException.NotFound(messages.Company_Not_Found());

            return result.Adapt<GetCompanyInfoResponse>();
        }
    }
}