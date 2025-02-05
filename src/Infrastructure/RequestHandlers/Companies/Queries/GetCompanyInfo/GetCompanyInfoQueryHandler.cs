using Mapster;
using MediatR;
using Shifty.Application.Companies.Queries.GetCompanyInfo;
using Shifty.Application.Companies.Responces.GetCompanyInfo;
using Shifty.Common.Exceptions;
using Shifty.Domain.Features.Setting;
using Shifty.Domain.Interfaces.Base;
using Shifty.Domain.Interfaces.Tenants.Companies;
using Shifty.Persistence.Repositories.Features.Settings.Queries;
using Shifty.Resources.Messages;

namespace Shifty.RequestHandlers.Companies.Queries.GetCompanyInfo
{
    public class GetCompanyInfoQueryHandler(ICompanyRepository companyRepository , CompanyMessages messages , ISettingQueriesRepository settingQueriesRepository ) : IRequestHandler<GetCompanyInfoQuery , GetCompanyInfoResponse>
    {
        public async Task<GetCompanyInfoResponse> Handle(GetCompanyInfoQuery request , CancellationToken cancellationToken)
        {
            if (!await companyRepository.IdentifierExistsAsync(request.Domain , cancellationToken))
                throw  ShiftyException.NotFound(messages.Company_Not_Found());

            var result = await companyRepository.GetByIdentifierAsync(request.Domain , cancellationToken);

            var setting = await settingQueriesRepository.GetSingleAsync(cancellationToken);

            if (result == null)
                throw ShiftyException.NotFound(messages.Company_Not_Found());

            return GetCompanyInfoResponse.Create(result , setting);
        }
    }
}