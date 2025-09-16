using SmartAttendance.Application.Features.Attendance.Requests;

namespace SmartAttendance.Application.Features.Attendance.Commands;

// Command: تایید معذوریت حضور
public sealed record ApproveExcuseCommand(
    Guid AttendanceId,
    ApproveExcuseRequest Request
) : IRequest;