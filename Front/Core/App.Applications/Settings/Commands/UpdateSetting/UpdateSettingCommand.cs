using App.Common.General.Enums.Setting;
using MediatR;

namespace App.Applications.Settings.Commands.UpdateSetting;

public record UpdateSettingCommand(
    List<SettingFlags> Flags
) : IRequest;