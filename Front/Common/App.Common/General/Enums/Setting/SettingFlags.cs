namespace App.Common.General.Enums.Setting;

[Flags]
public enum SettingFlags
{
    UniversityEnabled = 1 << 0,
    InitialStepper    = 1 << 1
}