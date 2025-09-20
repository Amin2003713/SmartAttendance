using App.Common.General.Enums.Setting;

namespace App.Applications.Settings.Requests.UpdateSetting;

public class UpdateSettingRequest
{
    public List<SettingFlags> Flags { get; set; }
}