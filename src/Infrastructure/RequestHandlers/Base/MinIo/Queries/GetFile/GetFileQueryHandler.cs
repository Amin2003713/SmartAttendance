using SmartAttendance.Application.Base.MinIo.Queries.GetFile;
using SmartAttendance.Application.Interfaces.Minio;
using SmartAttendance.Common.Exceptions;

namespace SmartAttendance.RequestHandlers.Base.MinIo.Queries.GetFile;

public class GetFileQueryHandler(
    IMinIoQueryRepository                 minIoQueryRepository,
    ILogger<GetFileQueryHandler>          logger,
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
                throw SmartAttendanceException.NotFound(localizer["Requested file not found."]);
            }

            logger.LogInformation("File retrieved: {FilePath}", request.FilePath);
            return stream;
        }
        catch (SmartAttendanceException ex)
        {
            logger.LogError(ex, "Business error while retrieving file {FilePath}.", request?.FilePath);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occurred while retrieving file {FilePath}.", request?.FilePath);

            throw SmartAttendanceException.InternalServerError(
                localizer["An unexpected error occurred while retrieving the file."]);
        }
    }
}