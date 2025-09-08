using SmartAttendance.Application.Interfaces.Base;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Messages.UserLikedMessages;

namespace SmartAttendance.Application.Interfaces.Messages.UserLikedMessages;

public interface IUserLikedMessageCommandRepository : ICommandRepository<UserLikedMessage>,
    IScopedDependency { }