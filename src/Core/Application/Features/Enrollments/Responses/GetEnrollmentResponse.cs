using SmartAttendance.Application.Features.Attendances.Responses;
using SmartAttendance.Common.Common.Responses.Users.Queries.Base;
using SmartAttendance.Common.General.Enums.Plans.Enrollment;

public class GetEnrollmentResponse
{
    public Guid PlanId { get; set; }
    public GetUserResponse? Student { get; set; }
    public EnrollmentStatus Status { get; set; }
    public DateTime EnrolledAt { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string Address  { get; set; }
    public GetAttendanceInfoResponse? Attendance  { get; set; }
    public string PlanName { get; set; } = default!;
}