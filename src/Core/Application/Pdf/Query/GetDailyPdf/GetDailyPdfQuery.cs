namespace Shifty.Application.Pdf.Query.GetDailyPdf;

public record GetDailyPdfQuery(
    
    DateTime Date
) : IRequest<string>;