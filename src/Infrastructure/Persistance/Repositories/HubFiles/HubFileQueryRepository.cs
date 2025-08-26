using System.IO;
using Shifty.Application.Base.HubFiles.Commands.UploadHubFile;
using Shifty.Application.Base.HubFiles.Commands.ZipExport;
using Shifty.Application.Base.HubFiles.Request.Queries.GetFile;
using Shifty.Application.Interfaces.HubFiles;
using Shifty.Application.Interfaces.Minio;
using Shifty.Common.General.Enums.FileType;
using Shifty.Common.Utilities.EnumHelpers;
using Shifty.Domain.HubFiles;

namespace Shifty.Persistence.Repositories.HubFiles;

public class HubFileQueryRepository(
    ReadOnlyDbContext dbContext,
    ILogger<HubFileQueryRepository> logger,
    IStringLocalizer<HubFileQueryRepository> localizer,
    IMinIoQueryRepository minIoQueryRepository,
    IMultiTenantContextAccessor<ShiftyTenantInfo> tenantContext
)
    : QueryRepository<HubFile>(dbContext, logger),
        IHubFileQueryRepository
{
    public async Task<FileTransferResponse> GetHubFile(
        Guid rowId,
        FileType fileType,
        FileStorageType storageType,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving hub file. FileId: {FileId}, FileType: {FileType}, StorageType: {StorageType}",
            rowId,
            fileType,
            storageType);

        var file = await TableNoTracking.FirstOrDefaultAsync(
            a => a.Id == rowId && a.Type == fileType && a.ReferenceIdType == storageType,
            cancellationToken);

        if (file == null)
        {
            logger.LogWarning("File not found. FileId: {FileId}", rowId);
            throw new NotFoundException(localizer["The requested file was not found."]);
        }

        var bytes = await GetFileBytArray(file.Path, cancellationToken);

        logger.LogInformation("Hub file retrieved successfully. FileName: {FileName}", file.Name);
        return new FileTransferResponse(bytes, file.Name!);
    }


    public async Task<List<HubFile?>> GetZipAsync(ZipExportCommand zipfile, CancellationToken cancellationToken)
    {
        // logger.LogInformation("Retrieving ZIP files for export. ProjectId: {ProjectId}, RowType: {RowType}",
        //     zipfile.ProjectId,
        //     zipfile.RowType);

        var files = await DbContext.Set<HubFile>()
            .Where(a => 
                // a.ProjectId == zipfile.ProjectId &&
                        a.ReferenceIdType == zipfile.RowType &&
                        a.ReportDate >= zipfile.FromDate &&
                        a.ReportDate <= zipfile.ToDate)
            .GroupBy(a => a.Name)
            .Select(a => a.OrderByDescending(w => w.ReportDate).FirstOrDefault())
            .ToListAsync(cancellationToken);

        logger.LogInformation("ZIP files retrieved. Count: {Count}", files.Count);
        return files;
    }


    public async Task<string> GetBucketPath(
        UploadHubFileCommand request,
        Guid? userId,
        CancellationToken cancellationToken)
    {
        // logger.LogInformation("Generating bucket path. ProjectId: {ProjectId}, UserId: {UserId}, Type: {Type}",
        //     request.ProjectId,
        //     userId,
        //     request.RowType);

        var path = $"tenant-{tenantContext.MultiTenantContext.TenantInfo?.Identifier}";

        if (request.RowType == FileStorageType.ProfilePicture)
            return Path.Combine(path, "User-Profiles").ToLower();


        var finalPath = Path.Combine(
                path,
                $"Date-{DateTime.UtcNow:yyyy-MM-dd}",
                $"User-{userId}",
                request.RowType.GetEnglishName()
            )
            .ToLower();

        logger.LogInformation("Bucket path generated: {Path}", finalPath);

        return finalPath;
    }

    private async Task<byte[]> GetFileBytArray(string filepath, CancellationToken cancellationToken)
    {
        logger.LogInformation("Reading file from MinIO. Path: {Path}", filepath);

        try
        {
            await using var fileStream = await minIoQueryRepository.GetFileAsync(filepath, cancellationToken);
            using var       ms         = new MemoryStream();
            await fileStream.CopyToAsync(ms, cancellationToken);
            logger.LogInformation("File read from MinIO successfully. Size: {Size} bytes", ms.Length);
            return ms.ToArray();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while reading file from MinIO. Path: {Path}", filepath);
            throw new NotFoundException(localizer["The requested file could not be retrieved from storage."]);
        }
    }
}