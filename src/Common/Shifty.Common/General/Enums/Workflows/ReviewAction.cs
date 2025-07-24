namespace Shifty.Common.General.Enums.Workflows;

public enum ReviewAction
{
    [Display(Name = "تأیید شده")] Verify,
    [Display(Name = "رد شده")]    Reject,
    [Display(Name = "جدید")]      New
}