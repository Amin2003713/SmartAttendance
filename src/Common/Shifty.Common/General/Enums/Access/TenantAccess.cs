namespace Shifty.Common.General.Enums.Access;

public enum TenantAccess
{
    Admin,

    [Display(Name = "دسترسی کاربر")] UserAccess,

    [Display(Name = "دسترسی به پیام‌ها")] MessageAccess,

    [Display(Name = "دسترسی به یادآوری‌ها")]
    ReminderAccess,
    [Display(Name = "دسترسی به تعطیلات")] HolidayAccess,

    [Display(Name = "دسترسی به صورتجلسات")]
    MeetingMinutes
}