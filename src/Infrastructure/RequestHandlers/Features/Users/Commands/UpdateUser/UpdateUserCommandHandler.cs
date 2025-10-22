using Mapster;
using Microsoft.AspNetCore.Identity;
using SmartAttendance.Application.Base.HubFiles.Commands.UploadHubFile;
using SmartAttendance.Application.Base.MinIo.Commands.DeleteFile;
using SmartAttendance.Application.Features.Users.Commands.UpdateUser;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Common.General.Enums.FileType;
using SmartAttendance.Domain.Users;
using SmartAttendance.Persistence.Services.Identities;

namespace SmartAttendance.RequestHandlers.Features.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler(
    UserManager<User>                          userManager,
    IdentityService                            identityService,
    IMediator                                  mediator,
    ILogger<UpdateUserCommandHandler>          logger,
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
                throw SmartAttendanceException.NotFound(localizer["User not found."]);
            }


            if (request.ProfilePicture?.MediaFile != null)
            {
                if (!string.IsNullOrWhiteSpace(user.ProfilePicture))
                {
                    var path = user.ProfilePicture.Replace("https://", "").Replace("http://", "");

                    var deleteResponse = await mediator.Send(new DeleteFileCommand(path),
                        cancellationToken);

                    if (!deleteResponse)
                    {
                        logger.LogError("Failed to delete old image for User {Id}.", user.Id);
                        throw SmartAttendanceException.InternalServerError(localizer["Failed to delete old image."].Value);
                    }
                }


                var uploadCommand = new UploadHubFileCommand
                {
                    File = request.ProfilePicture.MediaFile,
                };

                var uploadImageResponse = await mediator.Send(uploadCommand, cancellationToken);

                update.ProfilePicture = uploadImageResponse.Url;

                logger.LogInformation("Uploaded image for Profile{UserId}.", user.Id);
            }

            user.Update(update);

            await userManager.UpdateAsync(user);

            logger.LogInformation("User {TargetUserId} updated.", userId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating user {UserId}", userId);
            throw SmartAttendanceException.InternalServerError(localizer["An error occurred while updating the user."]);
        }
    }
}