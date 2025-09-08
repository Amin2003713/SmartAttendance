namespace SmartAttendance.Common.General.Enums.Workflows;

public enum ItemTypes : byte
{
    [Display(Name = "وضعیت آب و هوا")]     Weather,
    [Display(Name = "پیمانکار")]           Contractor,
    [Display(Name = "کارگر روزمزد")]       DailyWorker,
    [Display(Name = "پرسنل")]              Staff,
    [Display(Name = "ابزارها")]            Tool,
    [Display(Name = "مصالح")]              Material,
    [Display(Name = "تصاویر")]             Picture,
    [Display(Name = "جلسات")]              Session,
    [Display(Name = "مشکلات")]             Problem,
    [Display(Name = "پیشرفت پروژه")]       Progress,
    [Display(Name = "نامه‌ها")]            Letter,
    [Display(Name = "اقدامات مؤثر")]       EnhancementAction,
    [Display(Name = "پرداخت‌های مالی")]    FundPayment,
    [Display(Name = "اطلاعات مالی پروژه")] FinancialDetail,
    [Display(Name = "مجوزهای عملیاتی")]    WorkPermit
}