namespace Shifty.Domain.Setting;

public class Setting : BaseEntity
{
    public SettingFlags Flags { get; set; }

    public void Update(Setting entity)
    {
        Flags = entity.Flags;
    }
}