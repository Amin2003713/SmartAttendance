using Mapster;
using Microsoft.AspNetCore.Http;
using Shifty.Application.HubFiles.Commands.UploadHubFile;
using Shifty.Application.Interfaces.HubFiles;
using Shifty.Application.MinIo.Commands.UplodeFile;
using Shifty.Application.Storage.Commands.CreateStorage;
using Shifty.Application.Storage.Queries.GetAllRemainStorage;
using Shifty.Common.Exceptions;
using Shifty.Common.General;
using Shifty.Common.General.Enums.FileType;
using Shifty.Common.Utilities.EnumHelpers;
using Shifty.Common.Utilities.TypeConverters;
using Shifty.Domain.HubFiles;
using Shifty.Persistence.Services.Identities;

namespace Shifty.RequestHandlers.HubFiles.Commands.UploadHubFIle;

public class UploadHubFileCommandHandler(
    IHubFileCommandRepository hubFileCommandRepository,
    IMediator mediator,
    IdentityService identityService,
    IHubFileQueryRepository hubFileQueryRepository,
    ILogger<UploadHubFileCommandHandler> logger,
    IStringLocalizer<UploadHubFileCommandHandler> localizer
) : IRequestHandler<UploadHubFileCommand, MediaFileStorage>
{
    public async Task<MediaFileStorage> Handle(UploadHubFileCommand request, CancellationToken cancellationToken)
    {
        var userId = identityService.GetUserId();
        // logger.LogInformation("User {UserId} is uploading a file for ProjectId {ProjectId}.", userId, request.ProjectId);

        ValidateRequestFile(request);

        var fileSizeMb = request.File.Length.BytesToMegabytes();
        await EnsureStorageAvailableAsync(fileSizeMb, cancellationToken);

        var bucketPath = await hubFileQueryRepository.GetBucketPath(request, userId, cancellationToken);
        logger.LogInformation("Bucket path resolved: {BucketPath}", bucketPath);

        var uploadResult = await mediator.Send(UploadFileCommand.Create(request, bucketPath), cancellationToken);
        logger.LogInformation("File uploaded to MinIO. File Id: {Id}", uploadResult.Id);

        await TryUpdateStorageAsync(request, fileSizeMb, uploadResult.Type, cancellationToken);
        return await SaveFileRecordAsync(request, uploadResult, cancellationToken);
    }

    private void ValidateRequestFile(UploadHubFileCommand request)
    {
        if (request.File is null || request.File.Length == 0)
        {
            logger.LogWarning("File cannot be null or empty.");
            throw IpaException.BadRequest(localizer["File cannot be null or empty."].Value);
        }
    }

    private async Task EnsureStorageAvailableAsync(double fileSizeMb, CancellationToken cancellationToken)
    {
        var remain = await mediator.Send(new GetAllRemainStorageQuery(), cancellationToken);

        if (remain.AvailableStorageMb < (decimal)fileSizeMb)
        {
            logger.LogWarning("Insufficient storage. Required: {Required} MB, Available: {Available} MB.",
                fileSizeMb,
                remain.AvailableStorageMb);

            throw IpaException.BadRequest(localizer["Out of Storage."].Value);
        }
    }

    private async Task TryUpdateStorageAsync(UploadHubFileCommand request, double fileSizeMb, FileType fileType, CancellationToken cancellationToken)
    {
        if (request.RowType is FileStorageType.ZipExports or FileStorageType.PdfExports)
            return;

        var storageCommand = request.Adapt<CreateStorageCommand>().AddFileSize(fileSizeMb, fileType);
        await mediator.Send(storageCommand, cancellationToken);
        logger.LogInformation("Storage updated for {FileSizeMb} MB.", fileSizeMb);
    }

    private async Task<MediaFileStorage> SaveFileRecordAsync(UploadHubFileCommand request, HubFile path, CancellationToken cancellationToken)
    {
        try
        {
            await hubFileCommandRepository.AddAsync(path, cancellationToken);
            logger.LogInformation("File record added successfully. Id: {Id}", path.Id);

            var fileUrl = BuildFileUrl(path.Id, path.Type, path.ReferenceIdType);
            var type    = request.File.GetFileType();
            return new MediaFileStorage
            {
                Url = fileUrl.BuildImageUrl()!,
                FileName = request.File.FileName,
                Compressed = type == FileType.Picture ? fileUrl.BuildImageUrl(true) : null,
                Type = type
            };
        }
        catch (OutOfMemoryException oomEx)
        {
            // logger.LogError(oomEx, "Out of memory error while saving file record for ProjectId {ProjectId}.", request.ProjectId);
            throw IpaException.BadRequest(localizer["Your storage capacity has been reached."].Value);
        }
        catch (Exception ex)
        {
            // logger.LogError(ex, "Unexpected error while saving file record for ProjectId {ProjectId}.", request.ProjectId);
            throw IpaException.InternalServerError(localizer["An error occurred while uploading the file."].Value);
        }
    }

    private string BuildFileUrl(Guid id, FileType fileType, FileStorageType storageType)
    {
        return
            $"{identityService.TenantInfo!.Identifier}.{ApplicationConstant.Const.BaseDomain}/api/minio/hub-file/{id}?Type={fileType}&ReferenceType={storageType}&compress=False";
    }

    public static IFormFile ToFormFile(byte[] bytes, string fileName, string contentType = "application/octet-stream")
    {
        var stream = new MemoryStream(bytes);
        return new FormFile(stream, 0, bytes.Length, "file", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = contentType
        };
    }
}