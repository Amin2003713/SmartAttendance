using Shifty.Domain.Setting;

namespace Shifty.Application.Settings.Requests.UpdateSetting;

public class UpdateSettingRequest
{
    public List<SettingFlags> Flags { get; set; }
}