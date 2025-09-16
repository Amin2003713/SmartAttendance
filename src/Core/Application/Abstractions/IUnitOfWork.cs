namespace SmartAttendance.Application.Abstractions;

// واحد کار در لایه اپلیکیشن (بدون وابستگی به EF)
public interface IUnitOfWork
{
	/// <summary>
	/// ذخیره تغییرات تجمع‌ها. پیاده‌سازی در زیرساخت انجام می‌شود.
	/// </summary>
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

