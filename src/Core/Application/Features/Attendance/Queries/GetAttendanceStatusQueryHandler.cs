using SmartAttendance.Application.Features.Attendance.Responses;

namespace SmartAttendance.Application.Features.Attendance.Queries;

public sealed class GetAttendanceStatusQueryHandler(
    IAttendanceRepository repo
) : IRequestHandler<GetAttendanceStatusQuery, AttendanceDto>
{
    public async Task<AttendanceDto> Handle(GetAttendanceStatusQuery request, CancellationToken cancellationToken)
    {
        var att = await repo.FindByStudentPlanAsync(new UserId(request.StudentId), new PlanId(request.PlanId), cancellationToken) ??
                  throw new KeyNotFoundException("رکورد حضور یافت نشد.");

        return att.Adapt<AttendanceDto>();
    }
}