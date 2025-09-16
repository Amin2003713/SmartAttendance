using System.Threading;
using System.Threading.Tasks;
using SmartAttendance.Domain.Common;

namespace SmartAttendance.Domain.Services;

// سرویس دامنه: همگام‌سازی آفلاین (idempotent)
/// <summary>
/// مسئول همگام‌سازی آفلاین رکوردهای حضور. باید idempotent باشد.
/// </summary>
public interface IOfflineSyncService
{
	/// <summary>
	/// همگام‌سازی یک رکورد حضور. در صورت موفقیت true برمی‌گرداند.
	/// </summary>
	Task<bool> SyncAttendanceAsync(AttendanceId attendanceId, CancellationToken ct = default);
}

