using SmartAttendance.Application.Interfaces.Base;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Messages;

namespace SmartAttendance.Application.Interfaces.Messages;

public interface IMessageCommandRepository : ICommandRepository<Message>,
    IScopedDependency { }