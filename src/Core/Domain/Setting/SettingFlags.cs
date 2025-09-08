namespace SmartAttendance.Domain.Setting;

[Flags]
public enum SettingFlags
{
    CompanyEnabled = 1 << 0,
    InitialStepper = 1 << 1
}