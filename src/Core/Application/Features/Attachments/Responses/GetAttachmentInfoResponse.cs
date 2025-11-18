using SmartAttendance.Common.Common.Responses.Users.Queries.Base;

namespace SmartAttendance.Application.Features.Attachments.Responses;

public class GetAttachmentInfoResponse
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = default!;
    public string Url { get; set; } = default!;
    public string ContentType { get; set; } = default!;

    public Guid UploadedBy { get; set; }

    // Navigation
    public GetUserResponse? Uploader { get; set; } = default!;
}