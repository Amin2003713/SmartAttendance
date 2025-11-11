using SmartAttendance.Application.Features.Attachments.Responses;

public record GetAttachmentsByAttendanceIdQuery(
    Guid AttendanceId
) : IRequest<List<GetAttachmentInfoResponse>>;