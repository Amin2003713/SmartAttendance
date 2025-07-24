namespace Shifty.Application.Pdf.Query.GetDailyPdf;

public record GetDailyPdfQuery(
    Guid ProjectId,
    DateTime Date
) : IRequest<string>;