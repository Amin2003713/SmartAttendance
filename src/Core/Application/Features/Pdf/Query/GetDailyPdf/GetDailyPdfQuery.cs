namespace SmartAttendance.Application.Features.Pdf.Query.GetDailyPdf;

public record GetDailyPdfQuery(
    DateTime Date
) : IRequest<string>;