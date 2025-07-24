using Shifty.Domain.Setting;
using Swashbuckle.AspNetCore.Filters;

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