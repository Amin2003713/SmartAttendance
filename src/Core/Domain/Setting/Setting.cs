using SmartAttendance.Common.General.BaseClasses;

namespace SmartAttendance.Domain.Setting;

public class Setting : BaseEntity
{
    public SettingFlags Flags { get; set; }

    public void Update(Setting entity)
    {
        Flags = entity.Flags;
    }
}