using SmartAttendance.Application.Interfaces.Attachments;
using SmartAttendance.Domain.Features.Attachments;

namespace SmartAttendance.Persistence.Repositories.Features.Attachments;

public class AttachmentCommandRepository(
    WriteOnlyDbContext                       dbContext,
    ILogger<CommandRepository<Attachment>> logger
)
    : CommandRepository<Attachment>(dbContext, logger),
        IAttachmentCommandRepository { }