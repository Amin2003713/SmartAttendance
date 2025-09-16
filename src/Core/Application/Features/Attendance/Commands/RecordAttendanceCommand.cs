using SmartAttendance.Application.Features.Attendance.Requests;
using SmartAttendance.Application.Features.Attendance.Responses;

namespace SmartAttendance.Application.Features.Attendance.Commands;

// Command: ثبت حضور با یکی از روش‌ها (QR/GPS/Manual/Offline)
public sealed record RecordAttendanceCommand(
    RecordAttendanceRequest Request
) : IRequest<AttendanceDto>;

// Handler: ثبت حضور