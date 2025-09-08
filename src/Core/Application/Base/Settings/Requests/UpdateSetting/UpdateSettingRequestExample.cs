using SmartAttendance.Domain.Setting;

namespace SmartAttendance.Application.Base.Settings.Requests.UpdateSetting;

public class UpdateSettingRequestExample : IExamplesProvider<UpdateSettingRequest>
{
    public UpdateSettingRequest GetExamples()
    {
        return new UpdateSettingRequest
        {
            Flags =
            [
                SettingFlags.CompanyEnabled, SettingFlags.InitialStepper
            ]
        };
    }
}