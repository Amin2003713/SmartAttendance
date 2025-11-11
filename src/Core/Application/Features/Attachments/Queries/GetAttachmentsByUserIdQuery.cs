using SmartAttendance.Application.Features.Attachments.Responses;

public record GetAttachmentsByUserIdQuery(
    Guid UserId
) : IRequest<List<GetAttachmentInfoResponse>>;