using SmartAttendance.Application.Interfaces.Base;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Features.Attachments;

namespace SmartAttendance.Application.Interfaces.Attachments;

public interface IAttachmentCommandRepository : ICommandRepository<Attachment>,
      IScopedDependency { }