using SmartAttendance.Application.Features.Attendances.Responses;
using SmartAttendance.Application.Features.Majors.Responses;
using SmartAttendance.Application.Features.Subjects.Responses;
using SmartAttendance.Common.Common.Responses.Users.Queries.Base;

namespace SmartAttendance.Application.Features.Plans.Responses;

public class GetPlanInfoResponse
{
    public string CourseName { get; set; } = default!;

    public string Description { get; set; } = default!;
    public string Location { get; set; } = default!;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int Capacity { get; set; }

    public string Address { get; set; } = default!;


    public  GetMajorInfoResponse Major { get; set; }

    public List<GetSubjectInfoResponse> Subjects { get; set; } = [];
    public List<GetUserResponse> Teacher { get; set; } = [];

    public List<GetEnrollmentResponse> Enrollments { get; set; } = [];
    public List<GetAttendanceInfoResponse> Attendances { get; set; } = [];
}