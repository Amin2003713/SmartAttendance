# SmartAttendance.Domain

این پوشه شامل لایه دامنه (Domain Layer) مطابق DDD و Clean Architecture است.

## ساختار
- `Common`: انواع پایه مانند `Entity`, `AggregateRoot`, `IDomainEvent`, استثناها و شناسه‌های قوی‌تایپ.
- `ValueObjects`: VOها مانند `EmailAddress`, `PhoneNumber`, `NationalCode`, `GPSCoordinate`, `QRToken`, `PlanCapacity`, `AttendanceStatus`.
- `Events`: رویدادهای دامنه برای User/Plan/Attendance.
- `Services`: اینترفیس‌های سرویس دامنه (بدون پیاده‌سازی زیرساختی).
- `UserAggregate`, `PlanAggregate`, `AttendanceAggregate`: ریشه‌های تجمع با متدهای رفتاری.
- `DocumentAggregate`, `NotificationAggregate`: موجودیت‌های پشتیبان دامنه.
- `Docs`: مستندات دامنه (PlantUML, UseCases, BusinessRules, TestingGuide).

## اصول
- کد کاملاً مستقل از زیرساخت (EF/Db/IO/شبکه) است.
- همه اعتبارسنجی‌ها در VOها و Aggregateها انجام می‌شود.
- هر تغییر معنادار یک رویداد دامنه ایجاد می‌کند.
- شناسه‌ها قوی‌تایپ هستند (مانند `UserId`).

## انتگره‌سازی
- لایه Application از طریق Repositoryها Aggregate را ذخیره کرده و `DomainEvents` را منتشر می‌کند.
- سرویس‌های دامنه در لایه زیرساخت پیاده‌سازی و تزریق می‌شوند.