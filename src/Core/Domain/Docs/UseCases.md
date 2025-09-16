# Use Case Scenarios (Domain)

> این سناریوها به‌صورت دامنه‌محور نوشته شده‌اند و به زیرساخت وابسته نیستند.

## ثبت‌نام دانشجو در طرح

- بازیگران: Student, System
- پیش‌شرط: طرح فعال و ظرفیت خالی
- جریان اصلی:
    1. Student درخواست ثبت‌نام می‌دهد.
    2. `PlanAggregate.RegisterStudent(studentId)` فراخوانی می‌شود.
    3. در صورت پر نبودن ظرفیت، رزرو افزایش و رخداد `StudentRegisteredToPlanEvent` منتشر می‌شود.
    4. اگر ظرفیت پر شد، `PlanCapacityReachedEvent` منتشر می‌شود.
- مسیر جایگزین:
    - اگر ظرفیت پر بود، دانشجو به لیست انتظار اضافه می‌شود.

## لغو ثبت‌نام

- بازیگران: Student, System
- جریان:
    1. Student درخواست لغو می‌دهد.
    2. `PlanAggregate.CancelEnrollment(studentId)` اجرا می‌شود.
    3. ظرفیت آزاد و رخداد `EnrollmentCanceledEvent` منتشر می‌شود.
    4. اگر لیست انتظار خالی نبود، نفر بعدی ثبت‌نام می‌شود.

## ثبت حضور با QR

- بازیگران: Student, System
- جریان:
    1. Student QR را ارائه می‌کند.
    2. `AttendanceAggregate.RecordByQrAsync` با `IAttendanceValidationService` فراخوانی می‌شود.
    3. در صورت اعتبارسنجی موفق، وضعیت `Present` و رخدادهای `QRValidatedEvent` و `AttendanceRecordedEvent` منتشر می‌شود.

## ثبت حضور با GPS

- بازیگران: Student, System
- جریان:
    1. Student مختصات را ارسال می‌کند.
    2. `AttendanceAggregate.RecordByGpsAsync` با اعتبارسنجی فاصله.
    3. در صورت موفقیت، وضعیت `Present` و رخدادهای مرتبط منتشر می‌شود.

## ثبت دستی حضور

- بازیگران: Professor, System
- جریان:
    1. Professor درخواست ثبت دستی می‌دهد.
    2. `AttendanceAggregate.RecordManualAsync` با اعتبارسنجی RBAC.
    3. وضعیت به `Manual` یا `Absent` تغییر و رخداد ثبت می‌شود.

## تایید معذوریت

- بازیگران: Professor/Admin, System
- جریان:
    1. دلیل معذوریت ثبت می‌شود.
    2. `AttendanceAggregate.ApproveExcusal` وضعیت را به‌روزرسانی و `ExcusalApprovedEvent` منتشر می‌کند.

## محاسبه امتیاز

- بازیگران: System
- جریان:
    1. `AttendanceAggregate.CalculatePoints` با `IPointsCalculationService` اجرا می‌شود.
    2. رخداد `PointsCalculatedEvent` منتشر می‌شود.

## همگام‌سازی آفلاین

- بازیگران: System
- جریان:
    1. `AttendanceAggregate.RecordOffline` رکورد موقت می‌سازد.
    2. `AttendanceAggregate.SyncOfflineAsync` با `IOfflineSyncService` آن را به وضعیت قطعی ارتقا می‌دهد.