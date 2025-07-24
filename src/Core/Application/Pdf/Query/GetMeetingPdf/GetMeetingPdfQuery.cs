namespace Shifty.Application.Pdf.Query.GetMeetingPdf;

public record GetMeetingPdfQuery(
    Guid MeetingId,
    Guid ProjectId
) : IRequest<string>;