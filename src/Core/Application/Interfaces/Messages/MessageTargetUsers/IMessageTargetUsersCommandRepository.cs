using SmartAttendance.Application.Interfaces.Base;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Messages.MessageTargetUsers;

namespace SmartAttendance.Application.Interfaces.Messages.MessageTargetUsers;

public interface IMessageTargetUsersCommandRepository : ICommandRepository<MessageTargetUser>,
    IScopedDependency { }