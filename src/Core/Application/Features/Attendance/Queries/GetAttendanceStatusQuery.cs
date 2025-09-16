using SmartAttendance.Application.Features.Attendance.Responses;

namespace SmartAttendance.Application.Features.Attendance.Queries;

// Query: وضعیت حضور دانشجو در طرح
public sealed record GetAttendanceStatusQuery(
    Guid StudentId,
    Guid PlanId
) : IRequest<AttendanceDto>;