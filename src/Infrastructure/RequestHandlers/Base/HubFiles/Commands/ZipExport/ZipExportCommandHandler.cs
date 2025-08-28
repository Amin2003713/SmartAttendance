using Mapster;
using Microsoft.AspNetCore.Http;
using SharpCompress.Common;
using SharpCompress.Writers;
using Shifty.Application.Base.HubFiles.Commands.UploadHubFile;
using Shifty.Application.Base.HubFiles.Commands.ZipExport;
using Shifty.Application.Base.HubFiles.Request.Commands.ZipExport;
using Shifty.Application.Interfaces.HangFire;
using Shifty.Application.Interfaces.HubFiles;
using Shifty.Application.Interfaces.Minio;
using Shifty.Common.Exceptions;
using Shifty.Common.General;
using Shifty.Common.General.Enums.FileType;
using Shifty.Domain.HubFiles;

namespace Shifty.RequestHandlers.Base.HubFiles.Commands.ZipExport;

public class ZipExportCommandHandler(
    IHubFileQueryRepository hubFileQueryRepository,
    IMinIoQueryRepository minIoQueryRepository,
    IMediator mediator,
    IHangFireJobRepository jobRepository,
    IMinIoCommandRepository minIoCommandRepository,
    ILogger<ZipExportCommandHandler> logger,
    IStringLocalizer<ZipExportCommandHandler> localizer
)
    : IRequestHandler<ZipExportCommand, MediaFileStorage>
{
    public async Task<MediaFileStorage> Handle(ZipExportCommand request, CancellationToken cancellationToken)
    {
        // logger.LogInformation("Starting ZipExport for ProjectId {ProjectId}.", request.ProjectId);

        // 1) Retrieve files to be zipped.
        var files = await hubFileQueryRepository.GetZipAsync(request, cancellationToken);

        if (files == null || files.Count == 0)
        {
            // logger.LogWarning("No files found for zip export request in ProjectId {ProjectId}.", request.ProjectId);
            throw ShiftyException.BadRequest(localizer["No files available to export."].Value);
        }

        // 2) Create ZIP archive from retrieved files, with exception handling.
        IFormFile zip;

        try
        {
            zip = await CreateZipFromFiles(files, cancellationToken);
        }
        catch (Exception ex)
        {
            // logger.LogError(ex, "Failed to create ZIP archive for ProjectId {ProjectId}.", request.ProjectId);
            throw ShiftyException.InternalServerError(localizer["Failed to generate ZIP archive."].Value);
        }

        // 3) Prepare and send upload command.
        var dto = new ZipExportCommandResponse
        {
            File = zip,

            RowType = FileStorageType.ZipExports,
            RowId = Guid.NewGuid(),
            ReportDate = DateTime.UtcNow
        };

        var fileCommand = dto.Adapt<UploadHubFileCommand>();
        fileCommand.File = zip;

        MediaFileStorage path;

        try
        {
            path = await mediator.Send(fileCommand, cancellationToken);
        }
        catch (Exception ex)
        {
            // logger.LogError(ex, "Failed to upload ZIP archive for ProjectId {ProjectId}.", request.ProjectId);
            throw ShiftyException.InternalServerError(localizer["Failed to upload ZIP archive."].Value);
        }

        // 4) Schedule deletion of the temporary ZIP after 45 minutes.
        try
        {
            jobRepository.AddDelayedJob(
                () => minIoCommandRepository.DeleteFileAsync(path.Url, CancellationToken.None),
                TimeSpan.FromMinutes(45));

            logger.LogInformation("Scheduled deletion of ZIP file {Path} in 45 minutes.", path);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to schedule deletion of ZIP file {Path}.", path);
            throw ShiftyException.InternalServerError(localizer["Failed to schedule temporary file cleanup."].Value);
        }

        // logger.LogInformation("Zip export completed successfully for ProjectId {ProjectId}. Returning path.",
        //     request.ProjectId);

        return path;
    }

    private async Task<IFormFile> CreateZipFromFiles(List<HubFile> files, CancellationToken cancellationToken)
    {
        // Validate input
        if (files == null || files.Count == 0)
            throw ShiftyException.BadRequest(localizer["No files available to export."].Value);

        using var zipStream = new MemoryStream();

        using (var writer = WriterFactory.Open(zipStream, ArchiveType.Zip, CompressionType.Deflate))
        {
            foreach (var hubFile in files)
            {
                // 2a) Fetch each file, throwing if not found.
                Stream fileStream;

                try
                {
                    fileStream = await minIoQueryRepository.GetFileAsync(hubFile.Path, cancellationToken) ??
                                 throw ShiftyException.NotFound(
                                     localizer["File {0} not found in storage.", hubFile.Name].Value);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error retrieving file {FileName} from storage.", hubFile.Name);
                    throw ShiftyException.InternalServerError(
                        localizer["Error retrieving file {0} from storage.", hubFile.Name].Value);
                }

                // 2b) Skip zero-length files
                if (fileStream.Length == 0)
                {
                    logger.LogWarning("File {FileName} has zero length and will be skipped.", hubFile.Name);
                    continue;
                }

                // 2c) Write into zip, log any individual write errors
                try
                {
                    writer.Write(hubFile.Name!, fileStream);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error adding file {FileName} to ZIP.", hubFile.Name);
                    throw ShiftyException.InternalServerError(
                        localizer["Error adding file {0} to ZIP.", hubFile.Name].Value);
                }
            }
        }

        // Finalize ZIP
        zipStream.Position = 0;
        var zipBytes     = zipStream.ToArray();
        var resultStream = new MemoryStream(zipBytes);

        // Build the form file
        var zipFile = new FormFile(
            resultStream,
            0,
            resultStream.Length,
            "file",
            $"report_{files.First().Type}_{DateTime.UtcNow:yyyy_MM_dd_HH_mm_ss}.zip")
        {
            Headers = new HeaderDictionary(),
            ContentType = "application/zip"
        };

        return zipFile;
    }
}