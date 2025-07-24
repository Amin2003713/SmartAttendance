using Shifty.Domain.Setting;

namespace Shifty.Application.Settings.Commands.UpdateSetting;

public record UpdateSettingCommand(
    List<SettingFlags> Flags
) : IRequest;