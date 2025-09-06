namespace Shifty.Common.General.Enums;

public enum VehicleStatus : byte
{
    [Display(Name = "فعال")] Active,
    [Display(Name = "غیر فعال")] NotActive,
    [Display(Name = "درحال استفاده ")] Using
}