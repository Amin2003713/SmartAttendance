using SmartAttendance.Common.General.BaseClasses;
using SmartAttendance.Common.General.Enums.Excuse;
using SmartAttendance.Domain.Features.Attachments;
using SmartAttendance.Domain.Features.Attendances;
using SmartAttendance.Domain.Features.Plans;

namespace SmartAttendance.Domain.Features.Excuses;

public class Excuse : BaseEntity
{
    public Guid StudentId { get; set; }
    public Guid AttendanceId { get; set; }
    public string Reason { get; set; } = default!;
    public ExcuseStatus Status { get; set; } = ExcuseStatus.Pending;
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

    public Guid? AttachmentId { get; set; }

    // Navigation
    public User Student { get; set; } = default!;
    public Attachment? Attachment { get; set; }
    public Attendance? Attendance { get; set; }
}