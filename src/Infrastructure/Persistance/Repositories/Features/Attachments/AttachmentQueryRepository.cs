using SmartAttendance.Application.Interfaces.Attachments;
using SmartAttendance.Domain.Features.Attachments;

namespace SmartAttendance.Persistence.Repositories.Features.Attachments;

public class AttachmentQueryRepository(
    ReadOnlyDbContext                             dbContext,
    ILogger<QueryRepository<Attachment>>          logger,
    IStringLocalizer<Attachment> localizer
)
    : QueryRepository<Attachment>(dbContext, logger),
        IAttachmentQueryRepository;