namespace SmartAttendance.Common.General.Enums;

public enum MissionVehicleType : byte
{
    [Display(Name = "خودروی شخصی")] PersonalCar,
    [Display(Name = "خودروی شرکت")] CompanyCar,
    [Display(Name = "هواپیما")]     AirPlane,
    [Display(Name = "اتوبوس")]      Bus
}