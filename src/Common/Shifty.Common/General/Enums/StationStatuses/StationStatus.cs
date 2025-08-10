namespace Shifty.Common.General.Enums.Stationstatuses;

public enum StationStatus
{
    [Display(Name = "کامل")] Driver,
    [Display(Name = "ناقص")] Employee,
    [Display(Name = "هیجکدام")] None,
    [Display(Name = "خارج از محدوده")] OutOfState

}