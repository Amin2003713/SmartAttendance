using SmartAttendance.Application.Interfaces.Base;
using SmartAttendance.Domain.Users;

namespace SmartAttendance.Application.Interfaces.Users;

public interface IUserQueryRepository : IQueryRepository<User>;