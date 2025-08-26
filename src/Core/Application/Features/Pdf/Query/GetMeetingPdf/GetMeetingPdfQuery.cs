namespace Shifty.Application.Features.Pdf.Query.GetMeetingPdf;

public record GetMeetingPdfQuery(
    Guid MeetingId
) : IRequest<string>;