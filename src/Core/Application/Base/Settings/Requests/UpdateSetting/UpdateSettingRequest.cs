using Shifty.Domain.Setting;

namespace Shifty.Application.Base.Settings.Requests.UpdateSetting;

public class UpdateSettingRequest
{
    public List<SettingFlags> Flags { get; set; }
}