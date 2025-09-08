using SmartAttendance.Application.Base.HubFiles.Queries.GetById;
using SmartAttendance.Application.Base.MinIo.Commands.DeleteFile;
using SmartAttendance.Application.Interfaces.Minio;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Common.General;

namespace SmartAttendance.RequestHandlers.Base.MinIo.Commands.DeleteFile;

public class DeleteFileCommandHandler(
    IMinIoCommandRepository minIoCommandRepository,
    IMediator mediator,
    ILogger<DeleteFileCommandHandler> logger,
    IStringLocalizer<DeleteFileCommandHandler> localizer
)
    : IRequestHandler<DeleteFileCommand, bool>
{
    public async Task<bool> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request is null)
                throw new InvalidNullInputException(nameof(request));

            var path = request.FilePath.Replace(@"https://", "").Replace(@"http://", "");

            if (string.IsNullOrWhiteSpace(path))
            {
                logger.LogWarning("DeleteFile called with null or empty FilePath.");
                return false;
            }

            if (path.Contains("localhost") || path.Contains(ApplicationConstant.Const.BaseDomain))
            {
                var id = path.Split('/', StringSplitOptions.RemoveEmptyEntries)[4]
                    .Split("?", StringSplitOptions.RemoveEmptyEntries)[0];

                path = (await mediator.Send(new GetHubFileByIdQuery(Guid.Parse(id)), cancellationToken)).Path;
            }

            var result = await minIoCommandRepository.DeleteFileAsync(path, cancellationToken);

            if (result)
                logger.LogInformation("File {FilePath} deleted successfully.", path);
            else
                logger.LogWarning("File {FilePath} could not be deleted (may not exist).", path);

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while deleting file {FilePath}.", request?.FilePath);
            throw SmartAttendanceException.InternalServerError(localizer["An unexpected error occurred while deleting the file."]);
        }
    }
}