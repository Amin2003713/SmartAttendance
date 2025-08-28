using Mapster;
using Shifty.Application.Base.HubFiles.Commands.UploadHubFile;
using Shifty.Application.Features.Messages.Commands.CreateMessage;
using Shifty.Application.Interfaces.Messages;
using Shifty.Application.Interfaces.Messages.MessageTargetUsers;
using Shifty.Common.Exceptions;
using Shifty.Common.General.Enums.FileType;
using Shifty.Domain.Messages;
using Shifty.Domain.Messages.MessageTargetUsers;

namespace Shifty.RequestHandlers.Features.Messages.Commands.CreateMessage;

public class CreateMessageCommandHandler(
    IMessageCommandRepository messageCommandRepository,
    IMessageTargetUsersCommandRepository messageTargetUsersCommandRepository,
    ILogger<CreateMessageCommandHandler> logger,
    IStringLocalizer<CreateMessageCommandHandler> localizer,
    IMediator mediator
)
    : IRequestHandler<CreateCommand>
{
    public async Task Handle(CreateCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null)
                throw new InvalidNullInputException(nameof(request));

            var message = request.Adapt<Message>();

            if (request.ImageFile is { MediaFile: not null })
            {
                logger.LogInformation("Uploading image for message {MessageId} .",
                    message.Id);

                await using var imgStream = new MemoryStream();
                await request.ImageFile.MediaFile.CopyToAsync(imgStream, cancellationToken);


                var uploaded = await mediator.Send(new UploadHubFileCommand
                {
                    File = request.ImageFile.MediaFile,
                    RowId = message.Id,
                    RowType = FileStorageType.CompanyMessage
                }, cancellationToken);

                message.ImageUrl = uploaded.Url;


                await messageCommandRepository.AddAsync(message, cancellationToken);
                logger.LogInformation("Message {MessageId} created successfully.", message.Id);

                if (request.Recipients?.Any() == true)
                    foreach (var targetUserId in request.Recipients.Select(a => a.Id))
                    {
                        var messageTargetUser = new MessageTargetUser
                        {
                            MessageId = message.Id,
                            UserId = targetUserId
                        };

                        await messageTargetUsersCommandRepository.AddAsync(messageTargetUser, cancellationToken);
                        logger.LogInformation("User {UserId} added to message {MessageId}.", targetUserId, message.Id);
                    }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occurred while creating message.");
            throw ShiftyException.InternalServerError(
                localizer["An unexpected error occurred while creating the message."]);
        }
    }
}