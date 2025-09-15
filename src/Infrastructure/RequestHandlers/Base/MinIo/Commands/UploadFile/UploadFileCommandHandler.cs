using SmartAttendance.Application.Base.MinIo.Commands.UplodeFile;
using SmartAttendance.Application.Interfaces.Minio;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Domain.HubFiles;

namespace SmartAttendance.RequestHandlers.Base.MinIo.Commands.UploadFile;

public class UploadFileCommandHandler(
    IMinIoCommandRepository                    minIoCommandRepository,
    ILogger<UploadFileCommandHandler>          logger,
    IStringLocalizer<UploadFileCommandHandler> localizer
)
    : IRequestHandler<UploadFileCommand, HubFile>
{
    public async Task<HubFile> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request == null)
                throw new InvalidNullInputException(nameof(request));

            var result = await minIoCommandRepository.UploadFileAsync(request, cancellationToken);

            if (result != null)
            {
                logger.LogInformation("File uploaded successfully: {FileName}", result.Name);
                return result;
            }

            logger.LogWarning("Upload failed or returned null for file: {OriginalFileName}", request?.File?.FileName);
            throw SmartAttendanceException.BadRequest(localizer["File upload failed."]);
        }
        catch (SmartAttendanceException ex)
        {
            logger.LogError(ex, "Business error during file upload.");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error during file upload.");
            throw SmartAttendanceException.InternalServerError(localizer["An unexpected error occurred during file upload."]);
        }
    }
}