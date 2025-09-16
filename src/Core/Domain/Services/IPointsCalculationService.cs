using SmartAttendance.Domain.ValueObjects;

namespace SmartAttendance.Domain.Services;

// سرویس دامنه: محاسبه امتیاز حضور
/// <summary>
///     محاسبه امتیاز حضور/غیاب بر اساس وضعیت، تاخیر و معذوریت.
/// </summary>
public interface IPointsCalculationService
{
	/// <summary>
	///     محاسبه امتیاز نهایی. سیاست امتیازدهی قابل تغییر در زیرساخت.
	/// </summary>
	double CalculatePoints(AttendanceStatus status, TimeSpan? delay = null, bool isExcused = false);
}