using Shifty.Application.Base.Settings.Commands.UpdateSetting;
using Shifty.Application.Interfaces.Settings;
using Shifty.Common.Exceptions;
using Shifty.Common.Utilities.EnumHelpers;

namespace Shifty.RequestHandlers.Base.Settings.Commands.UpdateSetting;

public class UpdateSettingCommandHandler(
    ISettingCommandRepository repository,
    ISettingQueriesRepository queriesRepository,
    IStringLocalizer<UpdateSettingCommandHandler> localizer
)
    : IRequestHandler<UpdateSettingCommand>
{
    public async Task Handle(UpdateSettingCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var setting = await queriesRepository.GetSingleAsync(cancellationToken);
            if (setting == null)
                throw IpaException.NotFound(localizer["Setting Not Found."].Value);

            setting.Flags = 0;

            foreach (var flag in request.Flags)
            {
                setting.Flags = setting.Flags.AddFlag(flag);
            }

            await repository.UpdateAsync(setting, cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw IpaException.InternalServerError(localizer["toggle active command failed."].Value);
        }
    }
}