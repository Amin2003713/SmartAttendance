using SmartAttendance.Application.Features.Attendances.Responses;
using SmartAttendance.Common.Common.Responses.Users.Queries.Base;
using SmartAttendance.Common.General.Enums.Plans.Enrollment;

namespace SmartAttendance.Application.Features.Plans.Responses;

public class GetEnrollmentResponse
{
    public Guid Id { get; init; }
    public Guid PlanId { get; set; }
    public EnrollmentStatus Status { get; set; }
    public DateTime EnrolledAt { get; set; } 
    public GetAttendanceInfoResponse Attendance { get; set; } = null!;
    public GetUserResponse Student { get; init; }
}