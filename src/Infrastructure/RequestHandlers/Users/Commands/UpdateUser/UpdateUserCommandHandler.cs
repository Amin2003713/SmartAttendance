using Mapster;
using Microsoft.AspNetCore.Identity;
using Shifty.Application.Users.Commands.UpdateUser;
using Shifty.Common.Exceptions;
using Shifty.Common.General.Enums.FileType;

using Shifty.Common.Messaging.Contracts.MinIo.Minio.Commands;
using Shifty.Common.Utilities.TypeComverters;
using Shifty.Domain.Users;
using Shifty.Persistence.Services.Identities;

namespace Shifty.RequestHandlers.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler(
    UserManager<User> userManager,
    IdentityService identityService,
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
                throw IpaException.NotFound(localizer["User not found."]);
            }


            if (request.ImageFile?.MediaFile != null)
            {
                if (!string.IsNullOrWhiteSpace(user.Profile))
                {
                    var path = user.Profile.Replace("https://", "").Replace("http://", "");

                    var deleteResponse = await broker.RequestAsync<DeleteFileResponseBroker, DeleteFileCommandBroker>(
                        new DeleteFileCommandBroker(path),
                        cancellationToken);

                    if (!deleteResponse.IsDeleted)
                    {
                        logger.LogError("Failed to delete old image for User {Id}.", user.Id);
                        throw IpaException.InternalServerError(localizer["Failed to delete old image."].Value);
                    }
                }


                var uploadCommand = new UploadHubFileCommandBroker(
                    request.ImageFile.MediaFile.FileName,
                    await request.ImageFile.MediaFile.ToByte(cancellationToken),
                    null,
                    DateTime.Now.Date,
                    FileStorageType.ProfilePicture,
                    user.Id
                );

                var uploadResult = await broker.RequestAsync<UploadHubFileResponseBroker, UploadHubFileCommandBroker>(
                    uploadCommand,
                    cancellationToken);

                update.Profile = uploadResult.FileUrl;

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
            throw IpaException.InternalServerError(localizer["An error occurred while updating the user."]);
        }
    }
}