using SmartAttendance.Domain.AttendanceAggregate;

namespace SmartAttendance.Application.Abstractions;

public interface IAttendanceRepository
{
    Task<AttendanceAggregate?> GetByIdAsync(AttendanceId id, CancellationToken ct = default);
    Task                       AddAsync(AttendanceAggregate attendance, CancellationToken ct = default);
    Task                       UpdateAsync(AttendanceAggregate attendance, CancellationToken ct = default);
    Task                       DeleteAsync(AttendanceId id, CancellationToken ct = default);
    Task<AttendanceAggregate?> FindByStudentPlanAsync(UserId studentId, PlanId planId, CancellationToken ct = default);
}