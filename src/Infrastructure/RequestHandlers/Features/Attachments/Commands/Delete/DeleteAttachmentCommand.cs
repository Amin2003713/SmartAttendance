using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Base.MinIo.Commands.DeleteFile;
using SmartAttendance.Application.Base.MinIo.Commands.UplodeFile;
using SmartAttendance.Application.Features.Attachments.Commands.Create;
using SmartAttendance.Application.Interfaces.Attachments;
using SmartAttendance.Common.General.Enums;
using SmartAttendance.Persistence.Services.Identities;

namespace SmartAttendance.Application.Features.Attachments.Commands.Delete;

public class DeleteAttachmentCommandHandler(
    IMediator mediator ,
    IAttachmentCommandRepository attachmentCommandRepository ,
    IdentityService identityService,
    IAttachmentQueryRepository attachmentQueryRepository
) : IRequestHandler<DeleteAttachmentCommand>
{
    public async Task Handle(DeleteAttachmentCommand request, CancellationToken cancellationToken)
    {
        var attachments = await attachmentQueryRepository.TableNoTracking.Where(a => request.Ids.Contains(a.Id)).ToListAsync(cancellationToken);

        if (attachments.Count == 0)
            return;

        if (identityService.GetRoles() != Roles.Student)
            await attachmentCommandRepository.DeleteRangeAsync(attachments, cancellationToken);


        var userAttach = attachments.Where(a => a.UploadedBy == identityService.GetUserId<Guid>()).ToList();
        await attachmentCommandRepository.DeleteRangeAsync(userAttach, cancellationToken);
    }
}