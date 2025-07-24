using DNTPersianUtils.Core;
using Finbuckle.MultiTenant.Abstractions;
using Microsoft.AspNetCore.Identity;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Shifty.Application.Interfaces.Tenants.Payment;
using Shifty.Application.Interfaces.Tenants.Prices;
using Shifty.Application.Pdf.Query.GetFactorPdf;
using Shifty.Common.Exceptions;
using Shifty.Common.General.Enums.FileType;

using Shifty.Common.Messaging.Contracts.MinIo.HubFile.Commands.SavePdf;
using Shifty.Domain.Tenants;
using Shifty.Domain.Users;
using Shifty.Persistence.Services.Identities;
using Shifty.Persistence.Services.Pdf.GetFactorPdf;

namespace Shifty.RequestHandlers.Pdf.Queries.GetFactorPdf;

public class GetFactorPdfQueryHandler(
    IdentityService service,
    IPaymentQueryRepository paymentQueryRepository,
    IMultiTenantContextAccessor<ShiftyTenantInfo> accessor,
    IPriceQueryRepository priceQueryRepository,
    UserManager<User> userManager,
    ILogger<GetFactorPdfQueryHandler> logger,
    IStringLocalizer<GetFactorPdfQueryHandler> localizer
) : IRequestHandler<GetFactorPdfQuery, string>
{
    public async Task<string> Handle(GetFactorPdfQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var userId  = service.GetUserId<Guid>();
            var payment = await paymentQueryRepository.GetPaymentWithSuccess(request.PaymentId, cancellationToken);

            if (payment is null)
            {
                logger.LogWarning("Payment with ID {PaymentId} not found or not successful.", request.PaymentId);
                throw IpaException.NotFound(localizer["Payment not found."]);
            }

            var user = await userManager.FindByIdAsync(userId.ToString());

            if (user is null)
            {
                logger.LogWarning("User with ID {UserId} not found.", userId);
                throw IpaException.NotFound(localizer["User not found."]);
            }

            var price = await priceQueryRepository.GetPriceById(payment.PriceId, cancellationToken);
            QuestPDF.Settings.License = LicenseType.Community;

            var htmlContent = new GetFactorPdfDocument(accessor.MultiTenantContext.TenantInfo!,
                payment,
                broker,
                price);

            var pdf = htmlContent.GeneratePdf();

            var fileName = "فاکتور" +
                           DateTime.Now.ToShortPersianDateString().Replace("/", "_") +
                           ".pdf";

            var savePdfMessage = new SavePdfCommandBroker
            {
                File = pdf, FileName = fileName, RowType = FileStorageType.PdfExports, ProjectId = new Guid(payment.TenantId)
            };

            var result =
                await broker.RequestAsync<SavePdfCommandBrokerResponse, SavePdfCommandBroker>(savePdfMessage,
                    cancellationToken);

            logger.LogInformation(
                "PDF invoice generated successfully for user {UserId}, payment {PaymentId}. File: {FileName}",
                userId,
                request.PaymentId,
                fileName);

            return result.fileUrl;
        }
        catch (IpaException ex)
        {
            logger.LogError(ex, "Business error occurred while generating invoice PDF.");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Unexpected error occurred while generating invoice PDF for payment {PaymentId}.",
                request.PaymentId);

            throw IpaException.InternalServerError(
                localizer["An unexpected error occurred while generating the invoice PDF."]);
        }
    }
}