using SmartAttendance.Application.Features.Excuses.Responses;
using SmartAttendance.Common.Common.Responses.Users.Queries.Base;
using SmartAttendance.Common.General.Enums.Attendance;
using SmartAttendance.Domain.Features.Attendances;
using SmartAttendance.Domain.Features.Excuses;
using SmartAttendance.Domain.Features.Plans;
using SmartAttendance.Domain.Users;

namespace SmartAttendance.Application.Features.Attendances.Responses;

public class GetAttendanceInfoResponse
{
    public GetUserResponse Student { get; set; }

    public AttendanceStatus Status { get; set; }
    public DateTime RecordedAt { get; set; }

    public GetExcuseInfoResponse? Excuse { get; set; }
}