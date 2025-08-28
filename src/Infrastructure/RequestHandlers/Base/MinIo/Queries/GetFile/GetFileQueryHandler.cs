using Shifty.Application.Base.MinIo.Queries.GetFile;
using Shifty.Application.Interfaces.Minio;
using Shifty.Common.Exceptions;

namespace Shifty.RequestHandlers.Base.MinIo.Queries.GetFile;

public class GetFileQueryHandler(
    IMinIoQueryRepository minIoQueryRepository,
    ILogger<GetFileQueryHandler> logger,
    IStringLocalizer<GetFileQueryHandler> localizer
)
    : IRequestHandler<GetFileQuery, Stream>
{
    public async Task<Stream> Handle(GetFileQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request == null || string.IsNullOrWhiteSpace(request.FilePath))
                throw new InvalidNullInputException(nameof(request.FilePath));

            var stream = await minIoQueryRepository.GetFileAsync(request.FilePath, cancellationToken);

            if (stream == null)
            {
                logger.LogWarning("File not found: {FilePath}", request.FilePath);
                throw ShiftyException.NotFound(localizer["Requested file not found."]);
            }

            logger.LogInformation("File retrieved: {FilePath}", request.FilePath);
            return stream;
        }
        catch (ShiftyException ex)
        {
            logger.LogError(ex, "Business error while retrieving file {FilePath}.", request?.FilePath);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occurred while retrieving file {FilePath}.", request?.FilePath);
            throw ShiftyException.InternalServerError(
                localizer["An unexpected error occurred while retrieving the file."]);
        }
    }
}