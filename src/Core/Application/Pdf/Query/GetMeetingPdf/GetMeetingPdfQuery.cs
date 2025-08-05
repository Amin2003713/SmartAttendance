namespace Shifty.Application.Pdf.Query.GetMeetingPdf;

public record GetMeetingPdfQuery(
    Guid MeetingId
) : IRequest<string>;