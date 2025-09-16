namespace SmartAttendance.Application.Interfaces.Settings;

public interface ISettingCommandRepository : ICommandRepository<Setting>,
    IScopedDependency;