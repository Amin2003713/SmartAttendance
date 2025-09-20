using System.ComponentModel.DataAnnotations;

namespace App.Common.General.Enums.Pdf;

public enum PdfType
{
    [Display(Name = "گزارش روزانه")]   DailyPdf,
    [Display(Name = "گزارش صورتجلسه")] MeetingMinute,
    [Display(Name = "فاکتور")]         FactorPdf
}