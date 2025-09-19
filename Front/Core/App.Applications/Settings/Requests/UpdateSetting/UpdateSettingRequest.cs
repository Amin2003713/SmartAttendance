using SmartAttendance.Domain.Setting;

namespace SmartAttendance.Application.Base.Settings.Requests.UpdateSetting;

public class UpdateSettingRequest
{
    public List<SettingFlags> Flags { get; set; }
}