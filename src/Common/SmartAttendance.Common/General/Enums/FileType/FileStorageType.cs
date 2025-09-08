namespace SmartAttendance.Common.General.Enums.FileType;

public enum FileStorageType : byte
{
    [Display(Name = "مصالح")]              Material          = 5,
    [Display(Name = "تصاویر")]             Picture           = 6,
    [Display(Name = "جلسات")]              Session           = 7,
    [Display(Name = "مشکلات")]             Problem           = 8,
    [Display(Name = "نامه‌ها")]            Letter            = 10,
    [Display(Name = "اقدامات مؤثر")]       EnhancementAction = 11,
    [Display(Name = "پرداخت‌های مالی")]    FundPayment       = 12,
    [Display(Name = "اطلاعات مالی پروژه")] FinancialDetail   = 13,
    [Display(Name = "مجوزهای عملیاتی")]    WorkPermit        = 14,
    [Display(Name = "لوگوی شرکت")]         CompanyLogo       = 15,
    [Display(Name = "تصویر پروفایل")]      ProfilePicture    = 16,
    [Display(Name = "پیام‌های شرکت")]      CompanyMessage    = 17,
    [Display(Name = "پیام‌های پروژه")]     ProjectMessage    = 18,
    [Display(Name = "فایل‌های زیپ خروجی")] ZipExports        = 19,
    [Display(Name = "فایل‌های PDF خروجی")] PdfExports        = 20,

    [Display(Name = "فایل‌های EXCEL خروجی")]
    ExcelExports = 21,
    [Display(Name = "فایل‌های MSP خروجی")] MspExports = 22,

    [Display(Name = "فایل‌های Primavera خروجی")]
    PrimaveraExports = 23
}