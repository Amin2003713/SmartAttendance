using System.Threading;
using System.Threading.Tasks;
using SmartAttendance.Domain.ValueObjects;
using SmartAttendance.Domain.Common;

namespace SmartAttendance.Domain.Services;

// سرویس دامنه: اعتبارسنجی حضور (QR/GPS/دستی)
/// <summary>
/// سرویس اعتبارسنجی روندهای ثبت حضور. این سرویس باید در لایه زیرساخت پیاده‌سازی شود.
/// </summary>
public interface IAttendanceValidationService
{
	/// <summary>
	/// اعتبارسنجی توکن QR برای یک دانشجو و طرح مشخص.
	/// </summary>
	Task<bool> ValidateQrAsync(QRToken token, PlanId planId, UserId studentId, CancellationToken ct = default);

	/// <summary>
	/// بررسی صحت مکان دانشجو نسبت به درگاه ورود با شعاع مجاز بر حسب متر.
	/// </summary>
	Task<bool> ValidateGpsAsync(GPSCoordinate studentLocation, GPSCoordinate gateLocation, double allowedRadiusMeters, CancellationToken ct = default);

	/// <summary>
	/// اعتبارسنجی ثبت دستی حضور توسط تاییدکننده مجاز (RBAC).
	/// </summary>
	Task<bool> ValidateManualAsync(UserId approverId, CancellationToken ct = default);
}

