using SmartAttendance.Common.General.Enums.Plans.Enrollment;

namespace SmartAttendance.Application.Features.Notifications.Commands;

public class NotifyEnrollmentStatusChangedCommand : IRequest
{
    public Guid StudentId { get; set; }
    public string PlanId { get; set; }
    public EnrollmentStatus Status { get; set; }
}