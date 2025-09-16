using SmartAttendance.Domain.UserAggregate;

namespace SmartAttendance.Application.Abstractions;

// قراردادهای مخزن برای Aggregate Root ها در لایه اپلیکیشن
public interface IUserRepository
{
    Task<UserAggregate?> GetByIdAsync(UserId id, CancellationToken ct = default);
    Task                 AddAsync(UserAggregate user, CancellationToken ct = default);
    Task                 UpdateAsync(UserAggregate user, CancellationToken ct = default);
    Task                 DeleteAsync(UserId id, CancellationToken ct = default);
}

// ریپازیتوری برای مدیریت نقش‌ها (نوشتن/ایجاد)