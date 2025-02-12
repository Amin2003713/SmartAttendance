using Shifty.Domain.Common.BaseClasses;

namespace Shifty.Domain.Features.Setting;

public class Setting  : BaseEntity
{
    public SettingFlags Flags { get; set; }

    public void Update(Setting entity)
    {
        Flags = entity.Flags;
    }
}