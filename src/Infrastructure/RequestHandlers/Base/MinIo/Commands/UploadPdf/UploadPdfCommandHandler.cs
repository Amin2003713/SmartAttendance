using SmartAttendance.Application.Base.MinIo.Commands.UploadPdf;
using SmartAttendance.Application.Interfaces.Minio;
using SmartAttendance.Common.Exceptions;

namespace SmartAttendance.RequestHandlers.Base.MinIo.Commands.UploadPdf;

public class UploadPdfCommandHandler(
    IMinIoCommandRepository                   minIoCommandRepository,
    ILogger<UploadPdfCommandHandler>          logger,
    IStringLocalizer<UploadPdfCommandHandler> localizer
)
    : IRequestHandler<UploadPdfCommand, string>
{
    public async Task<string> Handle(UploadPdfCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null || string.IsNullOrWhiteSpace(request.Path))
                throw new InvalidNullInputException(nameof(request));

            await minIoCommandRepository.UploadPdfAsync(request, cancellationToken);

            logger.LogInformation("PDF uploaded successfully to path: {Path}", request.Path);

            return request.Path;
        }
        catch (SmartAttendanceException ex)
        {
            logger.LogError(ex, "Business error during PDF upload.");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error during PDF upload.");
            throw SmartAttendanceException.InternalServerError(localizer["An unexpected error occurred while uploading the PDF."]);
        }
    }
}