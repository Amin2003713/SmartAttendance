using Shifty.Domain.Setting;

namespace Shifty.Application.Settings.Requests.UpdateSetting;

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