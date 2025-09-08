using SmartAttendance.Application.Interfaces.Base;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Messages.UserVisitedMessages;

namespace SmartAttendance.Application.Interfaces.Messages.UserVisitedMessages;

public interface IUserVisitedMessageCommandRepository : ICommandRepository<UserVisitedMessage>,
    IScopedDependency { }