using SmartAttendance.Common.General.BaseClasses;

namespace SmartAttendance.Domain.Features.Attachments;

public class Attachment : BaseEntity
{
      public string FileName { get; set; } = default!;
      public string FilePath { get; set; } = default!;
      public string ContentType { get; set; } = default!;

      public Guid UploadedBy { get; set; }

      // Navigation
      public User Uploader { get; set; } = default!;
}