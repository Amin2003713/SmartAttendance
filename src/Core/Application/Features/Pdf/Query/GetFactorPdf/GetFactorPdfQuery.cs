namespace SmartAttendance.Application.Features.Pdf.Query.GetFactorPdf;

public record GetFactorPdfQuery(
    Guid PaymentId
) : IRequest<string>;