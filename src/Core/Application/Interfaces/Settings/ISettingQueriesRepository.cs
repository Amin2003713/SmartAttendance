namespace SmartAttendance.Application.Interfaces.Settings;

public interface ISettingQueriesRepository : IQueryRepository<Setting>,
                                             IScopedDependency { }