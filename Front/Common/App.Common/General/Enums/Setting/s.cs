namespace SmartAttendance.Domain.Setting;

[Flags]
public enum SettingFlags
{
    UniversityEnabled = 1 << 0,
    InitialStepper    = 1 << 1
}