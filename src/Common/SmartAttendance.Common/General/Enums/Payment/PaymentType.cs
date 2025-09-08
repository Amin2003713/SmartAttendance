namespace SmartAttendance.Common.General.Enums.Payment;

public enum PaymentType : byte
{
    [Display(Name = "افزایش فضای ذخیره‌سازی")]
    IncreaseStorage = 0,

    [Display(Name = "تمدید زودهنگام اشتراک شرکت")]
    RenewSubscriptionEarly = 1,
    [Display(Name = "افزودن کاربر جدید")] AddCompanyUser    = 2,
    [Display(Name = "تمدید اشتراک شرکت")] RenewSubscription = 3,

    [Display(Name = "اشتراک نسخه آزمایشی")]
    DemoSubscription = 4
}