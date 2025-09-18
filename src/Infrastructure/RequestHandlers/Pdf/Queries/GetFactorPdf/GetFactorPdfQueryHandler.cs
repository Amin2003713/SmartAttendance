// using DNTPersianUtils.Core;
// using Finbuckle.MultiTenant.Abstractions;
// using Microsoft.AspNetCore.Identity;
// using QuestPDF;
// using QuestPDF.Fluent;
// using QuestPDF.Infrastructure;
// using SmartAttendance.Application.Features.Pdf.Query.GetFactorPdf;
// using SmartAttendance.Application.Interfaces.Tenants.Payment;
// using SmartAttendance.Application.Interfaces.Tenants.Prices;
// using SmartAttendance.Common.Exceptions;
// using SmartAttendance.Domain.Tenants;
// using SmartAttendance.Domain.Users;
// using SmartAttendance.Persistence.Services.Identities;
// using SmartAttendance.Persistence.Services.Pdf.GetFactorPdf;
//
// namespace SmartAttendance.RequestHandlers.Pdf.Queries.GetFactorPdf;
//
// public class GetFactorPdfQueryHandler(
//     IdentityService service,
//     IPaymentQueryRepository paymentQueryRepository,
//     IMultiTenantContextAccessor<UniversityTenantInfo> accessor,
//     IPriceQueryRepository priceQueryRepository,
//     UserManager<User> userManager,
//     ILogger<GetFactorPdfQueryHandler> logger,
//     IStringLocalizer<GetFactorPdfQueryHandler> localizer
// ) : IRequestHandler<GetFactorPdfQuery, string>
// {
//     public async Task<string> Handle(GetFactorPdfQuery request, CancellationToken cancellationToken)
//     {
//         try
//         {
//             var userId  = service.GetUserId<Guid>();
//             var payment = await paymentQueryRepository.GetPaymentWithSuccess(request.PaymentId, cancellationToken);
//
//             if (payment is null)
//             {
//                 logger.LogWarning("Payment with ID {PaymentId} not found or not successful.", request.PaymentId);
//                 throw SmartAttendanceException.NotFound(localizer["Payment not found."]);
//             }
//
//             var user = await userManager.FindByIdAsync(userId.ToString());
//
//             if (user is null)
//             {
//                 logger.LogWarning("User with ID {UserId} not found.", userId);
//                 throw SmartAttendanceException.NotFound(localizer["User not found."]);
//             }
//
//             var price = await priceQueryRepository.GetPriceById(payment.PriceId, cancellationToken);
//             Settings.License = LicenseType.Community;
//
//             var htmlContent = new GetFactorPdfDocument(accessor.MultiTenantContext.TenantInfo!,
//                 payment,
//                 price);
//
//             var pdf = htmlContent.GeneratePdf();
//
//             var fileName = "فاکتور" +
//                            DateTime.UtcNow.ToShortPersianDateString().Replace("/", "_") +
//                            ".pdf";
//             //
//             // var savePdfMessage = new SavePdfCommand
//             // {
//             //     File = pdf,
//             //     FileName = fileName,
//             //     RowType = FileStorageType.PdfExports,
//
//             // };
//             //
//             // var result =
//             //     await me;
//
//             logger.LogInformation(
//                 "PDF invoice generated successfully for user {UserId}, payment {PaymentId}. File: {FileName}",
//                 userId,
//                 request.PaymentId,
//                 fileName);
//
//             return "result.fileUrl"; // todo : fix based on the mediator ;
//         }
//         catch (SmartAttendanceException ex)
//         {
//             logger.LogError(ex, "Business error occurred while generating invoice PDF.");
//             throw;
//         }
//         catch (Exception ex)
//         {
//             logger.LogError(ex,
//                 "Unexpected error occurred while generating invoice PDF for payment {PaymentId}.",
//                 request.PaymentId);
//
//             throw SmartAttendanceException.InternalServerError(
//                 localizer["An unexpected error occurred while generating the invoice PDF."]);
//         }
//     }
// }

