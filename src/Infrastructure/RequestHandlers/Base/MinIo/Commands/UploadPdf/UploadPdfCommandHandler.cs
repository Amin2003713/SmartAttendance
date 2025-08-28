using Shifty.Application.Base.MinIo.Commands.UploadPdf;
using Shifty.Application.Interfaces.Minio;
using Shifty.Common.Exceptions;

namespace Shifty.RequestHandlers.Base.MinIo.Commands.UploadPdf;

public class UploadPdfCommandHandler(
    IMinIoCommandRepository minIoCommandRepository,
    ILogger<UploadPdfCommandHandler> logger,
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
        catch (ShiftyException ex)
        {
            logger.LogError(ex, "Business error during PDF upload.");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error during PDF upload.");
            throw ShiftyException.InternalServerError(localizer["An unexpected error occurred while uploading the PDF."]);
        }
    }
}