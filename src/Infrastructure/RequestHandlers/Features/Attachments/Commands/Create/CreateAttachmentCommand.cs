using Mapster;
using SmartAttendance.Application.Base.MinIo.Commands.UplodeFile;
using SmartAttendance.Application.Interfaces.Attachments;
using SmartAttendance.Domain.Features.Attachments;
using SmartAttendance.Persistence.Services.Identities;

namespace SmartAttendance.Application.Features.Attachments.Commands.Create;

public class CreateAttachmentCommandHandler(
    IMediator mediator ,
    IAttachmentCommandRepository attachmentCommandRepository ,
    IdentityService identityService
) : IRequestHandler<CreateAttachmentCommand>
{
    public async Task Handle(CreateAttachmentCommand request, CancellationToken cancellationToken)
    {
        var file = await mediator.Send(new UploadFileCommand()
            {
                File = request.File,
                CreatedAt =  DateTime.Now,
                RowId = request.RowId
            } ,
            cancellationToken) ;


        var attachment = new Attachment()
        {
            FileName = request.File.FileName,
            Url = file.Path,
            ContentType = request.File.ContentType,
            UploadedBy = identityService.GetUserId<Guid>()
        }     ;


        await attachmentCommandRepository.AddAsync(attachment , cancellationToken);
    }
}