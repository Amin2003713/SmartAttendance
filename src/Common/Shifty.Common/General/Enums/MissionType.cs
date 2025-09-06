namespace Shifty.Common.General.Enums;

public enum MissionType : byte
{
    [Display(Name = "آموزشی")] Educational,
    [Display(Name = "فروش")] Sale,
    [Display(Name = "پشتیبانی")] Support,
    [Display(Name = "بازرسی")] Inspection,
    [Display(Name = "سایر")] Other
}