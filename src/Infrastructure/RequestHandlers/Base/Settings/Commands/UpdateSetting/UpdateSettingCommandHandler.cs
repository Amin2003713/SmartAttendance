using SmartAttendance.Application.Base.Settings.Commands.UpdateSetting;
using SmartAttendance.Application.Interfaces.Settings;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Common.Utilities.EnumHelpers;

namespace SmartAttendance.RequestHandlers.Base.Settings.Commands.UpdateSetting;

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
                throw SmartAttendanceException.NotFound(localizer["Setting Not Found."].Value);

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
            throw SmartAttendanceException.InternalServerError(localizer["toggle active command failed."].Value);
        }
    }
}