using Mapster;
using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Base.HubFiles.Commands.UploadHubFile;
using SmartAttendance.Application.Base.MinIo.Commands.DeleteFile;
using SmartAttendance.Application.Features.Messages.Commands.UpdateMessage;
using SmartAttendance.Application.Interfaces.Messages;
using SmartAttendance.Application.Interfaces.Messages.MessageTargetUsers;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Common.General.Enums.FileType;
using SmartAttendance.Domain.Messages;
using SmartAttendance.Domain.Messages.MessageTargetUsers;

namespace SmartAttendance.RequestHandlers.Features.Messages.Commands.UpdateMessage;

public class UpdateMessageCommandHandler(
    IMediator mediator,
    IMessageQueryRepository messageQueryRepository,
    IMessageCommandRepository messageCommandRepository,
    ILogger<UpdateMessageCommandHandler> logger,
    IStringLocalizer<UpdateMessageCommandHandler> localizer,
    IMessageTargetUsersCommandRepository messageTargetUsersCommand,
    IMessageTargetUsersQueryRepository messageTargetUsersQuery
) : IRequestHandler<UpdateMessageCommand>
{
    public async Task Handle(UpdateMessageCommand request, CancellationToken cancellationToken)
    {
        var message = await messageQueryRepository.GetSingleAsync(cancellationToken, a => a.Id == request.Id);

        if (message == null)
        {
            logger.LogWarning("Message with ID {MessageId} not found for update.", request.Id);
            throw SmartAttendanceException.NotFound(localizer["Message not found."]);
        }

        if (request.ImageFile is { MediaFile: not null })
        {
            if (!string.IsNullOrWhiteSpace(message.ImageUrl))
            {
                var deleteResponse = await mediator.Send(new DeleteFileCommand(message.ImageUrl),
                    cancellationToken);

                if (!deleteResponse)
                {
                    logger.LogError("Failed to delete old image for Message {Id}.", message.Id);
                    throw SmartAttendanceException.InternalServerError(localizer["Failed to delete old image."].Value);
                }
            }

            logger.LogInformation("Uploading image for message {MessageId}",
                message.Id);


            var uploaded = await mediator.Send(new UploadHubFileCommand
                {
                    File = request.ImageFile.MediaFile,
                    RowId = message.Id,
                    RowType = FileStorageType.CompanyMessage
                },
                cancellationToken);

            message.ImageUrl = uploaded.Url;
        }

        var updated = request.Adapt<Message>();
        updated.ImageUrl = message.ImageUrl;
        message.Update(updated);

        await messageCommandRepository.UpdateAsync(message, cancellationToken);
        logger.LogInformation("Message {MessageId} updated successfully.", message.Id);

        if (request.Recipients?.Any() == true)
        {
            var existingTargets = await messageTargetUsersQuery.Entities
                .Where(mt => mt.MessageId == message.Id)
                .ToListAsync(cancellationToken);

            if (existingTargets.Count != 0)
                await messageTargetUsersCommand.DeleteRangeAsync(
                    existingTargets,
                    cancellationToken
                );

            foreach (var userId in request.Recipients!.Select(r => r.Id))
            {
                await messageTargetUsersCommand.AddAsync(
                    new MessageTargetUser
                    {
                        MessageId = message.Id,
                        UserId = userId
                    },
                    cancellationToken
                );
            }
        }
    }
}