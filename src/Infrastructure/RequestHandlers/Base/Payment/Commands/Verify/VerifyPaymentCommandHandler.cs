using Finbuckle.MultiTenant.Abstractions;
using Microsoft.AspNetCore.Identity;
using SmartAttendance.Application.Base.Discounts.Commands.UseDiscount;
using SmartAttendance.Application.Base.Payment.Commands.Verify;
using SmartAttendance.Application.Base.ZarinPal.Request;
using SmartAttendance.Application.Interfaces.Tenants.Payment;
using SmartAttendance.Application.Interfaces.ZarinPal;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Common.General.Enums.Payment;
using SmartAttendance.Domain.Tenants;
using SmartAttendance.Domain.Users;

namespace SmartAttendance.RequestHandlers.Base.Payment.Commands.Verify;

public record VerifyPaymentCommandHandler(
    IZarinPal ZarinPal,
    IMediator Mediator,
    UserManager<User> UserManager,
    IPaymentCommandRepository CommandRepository,
    IPaymentQueryRepository QueryRepository,
    IMultiTenantContextAccessor<SmartAttendanceTenantInfo> ContextAccessor,
    ILogger<VerifyPaymentCommandHandler> Logger,
    IStringLocalizer<VerifyPaymentCommandHandler> Localizer
) : IRequestHandler<VerifyPaymentCommand, string>
{
    private SmartAttendanceTenantInfo TenantInfo => ContextAccessor.MultiTenantContext.TenantInfo!;

    public async Task<string> Handle(VerifyPaymentCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Authority) || string.IsNullOrWhiteSpace(request.Status))
        {
            Logger.LogWarning("Invalid verify request. Missing authority or status.");
            return RedirectToWeb(TenantInfo.GetUrl());
        }

        if (!request.Status.Contains("OK", StringComparison.OrdinalIgnoreCase))
        {
            Logger.LogWarning("Payment was canceled or not successful. StatusBroker: {StatusBroker}", request.Status);
            return RedirectToWeb(TenantInfo.GetUrl());
        }

        var companyPurchase = await QueryRepository.GetPayment(request.Authority, cancellationToken);

        if (companyPurchase is null)
        {
            Logger.LogWarning("Payment record not found for authority {Authority}.", request.Authority);
            return RedirectToWeb(TenantInfo.GetUrl());
        }

        if (companyPurchase.LastPaymentId is null)
        {
            Logger.LogError("LastPaymentId is null for authority {Authority}.", request.Authority);
            throw SmartAttendanceException.BadRequest(Localizer["Invalid payment reference."]);
        }

        var lastPurchase = await QueryRepository.GetPayment(companyPurchase.LastPaymentId.Value, cancellationToken);

        if (lastPurchase is null)
        {
            Logger.LogError("Last purchase not found for ID {LastPaymentId}.", companyPurchase.LastPaymentId);
            throw SmartAttendanceException.NotFound(Localizer["Previous payment record not found."]);
        }

        var cost = companyPurchase.PaymentType switch
                   {
                       PaymentType.IncreaseStorage => 0, PaymentType.RenewSubscriptionEarly or
                           PaymentType.AddCompanyUser or
                           PaymentType.RenewSubscription => companyPurchase.Cost,
                       PaymentType.DemoSubscription => 0, _ => throw SmartAttendanceException.BadRequest(Localizer["Invalid payment type."])
                   };

        var verifyResponse = await ZarinPal.VerifyPayment(
            new ZarinPalVerifyRequest((long)cost, request.Status, request.Authority),
            cancellationToken);

        companyPurchase.RefId = verifyResponse.RefId.ToString();
        companyPurchase.Status = verifyResponse.Code!.Value;
        companyPurchase.IsActive = true;
        lastPurchase.IsActive = false;

        if (companyPurchase.DiscountId is not null)
        {
            Logger.LogInformation("Applying discount for purchase {PurchaseId}.", companyPurchase.Id);
            await Mediator.Send(new UseDiscountCommand(companyPurchase), cancellationToken);
        }

        await CommandRepository.Update(lastPurchase,    cancellationToken);
        await CommandRepository.Update(companyPurchase, cancellationToken);

        Logger.LogInformation("Payment verified successfully. RefId: {RefId}", verifyResponse.RefId);

        return RedirectToWeb(TenantInfo.GetUrl(), verifyResponse.Code.ToString(), verifyResponse.RefId.ToString());
    }

    private string RedirectToWeb(Uri? url, string code = "", string refId = "")
    {
        var baseUrl = url?.AbsoluteUri.Replace("/api", string.Empty).TrimEnd('/');
        return $"{baseUrl}?code={code}&RefId={refId}";
    }
}