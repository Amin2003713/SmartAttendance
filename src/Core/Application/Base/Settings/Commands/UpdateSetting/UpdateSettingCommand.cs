namespace SmartAttendance.Application.Base.Settings.Commands.UpdateSetting;

public record UpdateSettingCommand(
    List<SettingFlags> Flags
) : IRequest;