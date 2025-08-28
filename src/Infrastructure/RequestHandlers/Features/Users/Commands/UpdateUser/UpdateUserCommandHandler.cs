using Mapster;
using Microsoft.AspNetCore.Identity;
using Shifty.Application.Base.HubFiles.Commands.UploadHubFile;
using Shifty.Application.Base.MinIo.Commands.DeleteFile;
using Shifty.Application.Features.Users.Commands.UpdateUser;
using Shifty.Common.Exceptions;
using Shifty.Common.General.Enums.FileType;
using Shifty.Domain.Users;
using Shifty.Persistence.Services.Identities;

namespace Shifty.RequestHandlers.Features.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler(
    UserManager<User> userManager,
    IdentityService identityService,
    IMediator mediator,
    ILogger<UpdateUserCommandHandler> logger,
    IStringLocalizer<UpdateUserCommandHandler> localizer
)
    : IRequestHandler<UpdateUserCommand>
{
    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var userId = identityService.GetUserId<Guid>();
        var user   = await userManager.FindByIdAsync(userId.ToString());
        var update = request.Adapt<User>();

        try
        {
            if (user == null)
            {
                logger.LogWarning("User with ID {UserId} not found.", userId);
                throw ShiftyException.NotFound(localizer["User not found."]);
            }


            if (request.ImageFile?.MediaFile != null)
            {
                if (!string.IsNullOrWhiteSpace(user.Profile))
                {
                    var path = user.Profile.Replace("https://", "").Replace("http://", "");

                    var deleteResponse = await mediator.Send(new DeleteFileCommand(path),
                        cancellationToken);

                    if (!deleteResponse)
                    {
                        logger.LogError("Failed to delete old image for User {Id}.", user.Id);
                        throw ShiftyException.InternalServerError(localizer["Failed to delete old image."].Value);
                    }
                }


                var uploadCommand = new UploadHubFileCommand
                {
                    File = request.ImageFile.MediaFile,

                    ReportDate = DateTime.UtcNow,
                    RowType = FileStorageType.ProfilePicture,
                    RowId = userId
                };

                var uploadImageResponse = await mediator.Send(uploadCommand, cancellationToken);

                update.Profile = uploadImageResponse.Url;

                logger.LogInformation("Uploaded image for Profile{UserId}.", user.Id);
            }

            user.Update(update);

            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                user.Email = request.Email;
                user.NormalizedEmail = request.Email.ToUpperInvariant();
            }

            await userManager.UpdateAsync(user);

            logger.LogInformation("User {TargetUserId} updated.", userId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating user {UserId}", userId);
            throw ShiftyException.InternalServerError(localizer["An error occurred while updating the user."]);
        }
    }
}