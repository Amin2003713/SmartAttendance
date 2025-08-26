using Shifty.Domain.Setting;

namespace Shifty.Application.Base.Settings.Commands.UpdateSetting;

public record UpdateSettingCommand(
    List<SettingFlags> Flags
) : IRequest;