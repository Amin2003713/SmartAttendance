namespace SmartAttendance.Domain.Defaults;

public abstract class TenantDefaultValue
{
    public static Setting.Setting Setting()
    {
        return new Setting.Setting
        {
            Id    = Guid.Parse("A360ED40-C440-4258-BF8A-D78B71AD390C"),
            Flags = SettingFlags.CompanyEnabled.AddFlag(SettingFlags.InitialStepper)
        };
    }
}