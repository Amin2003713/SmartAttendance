using System.Collections.Generic;
using SmartAttendance.Domain.UserAggregate;
using SmartAttendance.Domain.PlanAggregate;
using SmartAttendance.Domain.AttendanceAggregate;
using SmartAttendance.Domain.DocumentAggregate;
using SmartAttendance.Domain.NotificationAggregate;

namespace SmartAttendance.Application.Abstractions;

// قراردادهای مخزن برای Aggregate Root ها در لایه اپلیکیشن
public interface IUserRepository
{
	Task<UserAggregate?> GetByIdAsync(UserId id, CancellationToken ct = default);
	Task AddAsync(UserAggregate user, CancellationToken ct = default);
	Task UpdateAsync(UserAggregate user, CancellationToken ct = default);
	Task DeleteAsync(UserId id, CancellationToken ct = default);
}

public interface IRoleReadService
{
	Task<bool> RoleExistsAsync(string roleName, CancellationToken ct = default);
	Task<(RoleId Id, string Name)?> GetByIdAsync(RoleId id, CancellationToken ct = default);
	Task<IReadOnlyList<(RoleId Id, string Name)>> ListAsync(CancellationToken ct = default);
}

public interface IPlanRepository
{
	Task<PlanAggregate?> GetByIdAsync(PlanId id, CancellationToken ct = default);
	Task AddAsync(PlanAggregate plan, CancellationToken ct = default);
	Task UpdateAsync(PlanAggregate plan, CancellationToken ct = default);
	Task DeleteAsync(PlanId id, CancellationToken ct = default);
	Task<IReadOnlyList<PlanAggregate>> ListAsync(CancellationToken ct = default);
}

public interface IAttendanceRepository
{
	Task<AttendanceAggregate?> GetByIdAsync(AttendanceId id, CancellationToken ct = default);
	Task AddAsync(AttendanceAggregate attendance, CancellationToken ct = default);
	Task UpdateAsync(AttendanceAggregate attendance, CancellationToken ct = default);
	Task DeleteAsync(AttendanceId id, CancellationToken ct = default);
	Task<AttendanceAggregate?> FindByStudentPlanAsync(UserId studentId, PlanId planId, CancellationToken ct = default);
}

public interface IDocumentRepository
{
	Task<Document?> GetByIdAsync(DocumentId id, CancellationToken ct = default);
	Task AddAsync(Document document, CancellationToken ct = default);
	Task DeleteAsync(DocumentId id, CancellationToken ct = default);
}

public interface INotificationRepository
{
	Task<Notification?> GetByIdAsync(NotificationId id, CancellationToken ct = default);
	Task AddAsync(Notification notification, CancellationToken ct = default);
	Task UpdateAsync(Notification notification, CancellationToken ct = default);
	Task DeleteAsync(NotificationId id, CancellationToken ct = default);
	Task<IReadOnlyList<Notification>> GetByUserAsync(UserId userId, CancellationToken ct = default);
}

// ریپازیتوری برای مدیریت نقش‌ها (نوشتن/ایجاد)
public interface IRoleRepository
{
	Task<bool> ExistsByNameAsync(string roleName, CancellationToken ct = default);
	Task AddAsync(RoleId id, string name, CancellationToken ct = default);
}

