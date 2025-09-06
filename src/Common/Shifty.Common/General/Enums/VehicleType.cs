namespace Shifty.Common.General.Enums;

public enum VehicleType : byte
{
    [Display(Name = "خودروی شخصی")] Car,
    [Display(Name = "خودروی شرکت")] Motorcycle,
    [Display(Name = "هواپیما")] Van,
    [Display(Name = "اتوبوس")] Truck
}