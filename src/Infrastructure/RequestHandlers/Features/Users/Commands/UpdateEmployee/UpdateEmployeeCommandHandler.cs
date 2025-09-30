using Microsoft.EntityFrameworkCore;
using SmartAttendance.Application.Base.HubFiles.Commands.UploadHubFile;
using SmartAttendance.Application.Base.MinIo.Commands.DeleteFile;
using SmartAttendance.Application.Features.Users.Commands.AddRole;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Common.General.Enums.FileType;
using SmartAttendance.Persistence.Db;

namespace SmartAttendance.RequestHandlers.Features.Users.Commands.UpdateEmployee;

public class UpdateEmployeeCommandHandler (
    SmartAttendanceDbContext dbContext,
    IMediator mediator,
    ILogger<UpdateEmployeeCommandHandler> logger
)
    : IRequestHandler<UpdateEmployeeCommand>
{
    public async Task Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        // Find existing user
        var user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null)
            throw SmartAttendanceException.NotFound($"User with Id {request.UserId} not found.");

        // Update basic properties
        user.FirstName   = request.FirstName ?? user.FirstName;
        user.LastName    = request.LastName ?? user.LastName;
        user.FatherName  = request.FatherName ?? user.FatherName;
        user.NationalCode = request.NationalCode ?? user.NationalCode;
        user.PhoneNumber = request.PhoneNumber ?? user.PhoneNumber;
        user.BirthDate   = request.BirthDate ?? user.BirthDate;
        user.Address     = request.Address ?? user.Address;
        user.Gender      = request.Gender;
        user.IsActive    = request.IsActive ?? user.IsActive;

        // Handle profile picture
        if (request.ProfilePicture?.MediaFile != null)
        {
            // Delete old profile picture if exists
            if (!string.IsNullOrWhiteSpace(user.ProfilePicture))
            {
                var oldPath      = user.ProfilePicture!.Replace("https://", "").Replace("http://", "");
                var deleteResult = await mediator.Send(new DeleteFileCommand(oldPath), cancellationToken);

                if (!deleteResult)
                {
                    logger.LogError("Failed to delete old profile picture for user {UserId}.", user.Id);
                    throw SmartAttendanceException.InternalServerError("Failed to delete old profile picture.");
                }
            }

            // Upload new profile picture
            var uploadCommand = new UploadHubFileCommand
            {
                File       = request.ProfilePicture.MediaFile,
                ReportDate = DateTime.UtcNow,
                RowType    = FileStorageType.ProfilePicture,
                RowId      = user.Id
            };

            var uploadResponse = await mediator.Send(uploadCommand, cancellationToken);
            user.ProfilePicture = uploadResponse.Url;
            logger.LogInformation("Uploaded new profile picture for user {UserId}.", user.Id);
        }

        dbContext.Users.Update(user);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}