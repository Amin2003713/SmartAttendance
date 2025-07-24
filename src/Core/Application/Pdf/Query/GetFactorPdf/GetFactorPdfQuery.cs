namespace Shifty.Application.Pdf.Query.GetFactorPdf;

public record GetFactorPdfQuery(
    Guid PaymentId
) : IRequest<string>;