using SmartAttendance.Common.General.BaseClasses;

namespace SmartAttendance.Domain.Features.Attachments;

public class Attachment : BaseEntity
{
    public string FileName { get; set; } = default!;
    public string Url { get; set; } = default!;
    public string ContentType { get; set; } = default!;


    public Guid UploadedBy { get; set; }
    public User Uploader { get; set; } = default!;
}