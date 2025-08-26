using Shifty.Application.Base.Companies.Queries.GetCompanyInfo;
using Shifty.Application.Base.Companies.Responses.GetCompanyInfo;
using Shifty.Application.Commons.Queries.GetLogPropertyInfo.OperatorLogs;
using Shifty.Application.Interfaces.Settings;
using Shifty.Application.Interfaces.Tenants.Companies;
using Shifty.Application.Interfaces.Tenants.Payment;
using Shifty.Common.Exceptions;

namespace Shifty.RequestHandlers.Base.Companies.Queries.GetCompanyInfo;

public class GetCompanyInfoQueryHandler(
    ICompanyRepository companyRepository,
    ISettingQueriesRepository settingQueriesRepository,
    IPaymentQueryRepository paymentQueriesRepository,
    IMediator mediator,
    IStringLocalizer<GetCompanyInfoQueryHandler> localizer
)
    : IRequestHandler<GetCompanyInfoQuery, GetCompanyInfoResponse>
{
    public async Task<GetCompanyInfoResponse> Handle(GetCompanyInfoQuery request, CancellationToken cancellationToken)
    {
        if (!await companyRepository.IdentifierExistsAsync(request.Domain, cancellationToken))
            throw IpaException.NotFound(localizer["Company not found."].Value); // "شرکت یافت نشد."

        var result  = await companyRepository.GetByIdentifierAsync(request.Domain, cancellationToken);
        var setting = await settingQueriesRepository.GetSingleAsync(cancellationToken);
        var payment = await paymentQueriesRepository.GetPayment(cancellationToken);
        var creator = await mediator.Send(new GetLogPropertyInfoQuery(result.UserId!.Value), cancellationToken);

        if (result == null)
            throw IpaException.NotFound(localizer["Company not found."].Value); // "شرکت یافت نشد."

        return GetCompanyInfoResponse.Create(result, setting, payment?.LeftDays() ?? 0, creator);
    }
}