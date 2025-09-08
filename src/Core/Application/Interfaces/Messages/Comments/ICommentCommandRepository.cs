using SmartAttendance.Application.Interfaces.Base;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Messages.Comments;

namespace SmartAttendance.Application.Interfaces.Messages.Comments;

public interface ICommentCommandRepository : ICommandRepository<Comment>,
    IScopedDependency { }