# راهنمای تست دامنه

> تست‌ها باید فقط روی لایه دامنه تمرکز کنند و وابستگی به زیرساخت نداشته باشند.

## پیشنهادات کلی

- از تست‌های واحد برای VOها و قوانین سازنده استفاده کنید.
- برای Aggregateها از تست‌های رفتاری (Given/When/Then) بنویسید.
- رویدادهای دامنه را بعد از هر عمل بررسی کنید.
- سرویس‌های دامنه را با Double/Fake تست کنید.

## نمونه‌ها

### EmailAddress

- ورودی نامعتبر: خالی، طول > 254، الگوی اشتباه.
- ورودی معتبر: `user@example.com`.

### PlanAggregate.RegisterStudent

- Given ظرفیت کافی، When ثبت‌نام، Then دانشجو به لیست دانشجویان اضافه و رویداد `StudentRegisteredToPlanEvent` منتشر
  می‌شود.
- Given ظرفیت پر، When ثبت‌نام، Then به صف انتظار اضافه می‌شود.

### AttendanceAggregate.RecordByQrAsync

- Given QR معتبر، When ثبت، Then وضعیت `Present` و رویدادهای `QRValidatedEvent` و `AttendanceRecordedEvent` ثبت می‌شود.
- Given QR نامعتبر، Then استثنای `BusinessRuleViolationException` رخ می‌دهد.

### ApproveExcusal

- Given وضعیت `Absent`، When تایید معذوریت، Then وضعیت `Excused` و رویداد `ExcusalApprovedEvent`.

## نکات پیاده‌سازی تست

- از `AggregateRoot.DomainEvents` برای assert رویدادها استفاده کنید.
- بعد از انتشار، میتوانید `ClearDomainEvents()` را فراخوانی کنید.
- برای سرویس‌ها از واسط‌ها استفاده و رفتارها را شبیه‌سازی کنید.