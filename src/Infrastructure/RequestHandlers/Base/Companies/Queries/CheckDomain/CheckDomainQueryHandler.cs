using SmartAttendance.Application.Base.Companies.Queries.CheckDomain;
using SmartAttendance.Application.Interfaces.Tenants.Companies;
using SmartAttendance.Common.Exceptions;
using CancellationToken = System.Threading.CancellationToken;

namespace SmartAttendance.RequestHandlers.Base.Companies.Queries.CheckDomain;

public class CheckDomainQueryHandler(
    ICompanyRepository companyRepository,
    IStringLocalizer<CheckDomainQueryHandler> localizer
)
    : IRequestHandler<CheckDomainQuery, CheckDomainResponse>
{
    public async Task<CheckDomainResponse> Handle(CheckDomainQuery request, CancellationToken cancellationToken)
    {
        if (request is null)
            throw new InvalidNullInputException(nameof(request));

        var validation = await companyRepository.ValidateDomain(request.Domain, cancellationToken);

        return new CheckDomainResponse(
            validation,
            validation ? localizer["Tenant domain is not valid."].Value : localizer["Tenant domain is valid."].Value
        );
    }
}