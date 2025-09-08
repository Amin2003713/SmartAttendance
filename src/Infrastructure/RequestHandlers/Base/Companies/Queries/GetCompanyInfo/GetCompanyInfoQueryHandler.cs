using SmartAttendance.Application.Base.Companies.Queries.GetCompanyInfo;
using SmartAttendance.Application.Base.Companies.Responses.GetCompanyInfo;
using SmartAttendance.Application.Commons.Queries.GetLogPropertyInfo.OperatorLogs;
using SmartAttendance.Application.Interfaces.Settings;
using SmartAttendance.Application.Interfaces.Tenants.Companies;
using SmartAttendance.Application.Interfaces.Tenants.Payment;
using SmartAttendance.Common.Exceptions;

namespace SmartAttendance.RequestHandlers.Base.Companies.Queries.GetCompanyInfo;

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
            throw SmartAttendanceException.NotFound(localizer["Company not found."].Value); // "شرکت یافت نشد."

        var result  = await companyRepository.GetByIdentifierAsync(request.Domain, cancellationToken);
        var setting = await settingQueriesRepository.GetSingleAsync(cancellationToken);
        var payment = await paymentQueriesRepository.GetPayment(cancellationToken);
        var creator = await mediator.Send(new GetLogPropertyInfoQuery(result.UserId!.Value), cancellationToken);

        if (result == null)
            throw SmartAttendanceException.NotFound(localizer["Company not found."].Value); // "شرکت یافت نشد."

        return GetCompanyInfoResponse.Create(result, setting, payment?.LeftDays() ?? 0, creator);
    }
}