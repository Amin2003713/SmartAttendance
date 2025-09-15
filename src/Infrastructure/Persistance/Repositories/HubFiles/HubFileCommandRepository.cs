using System.IO;
using SharpCompress.Common;
using SmartAttendance.Application.Base.HubFiles.Commands.ZipExport;
using SmartAttendance.Application.Base.HubFiles.Request.Commands.ZipExport;
using SmartAttendance.Application.Base.MinIo.Commands.UploadPdf;
using SmartAttendance.Application.Base.MinIo.Commands.UplodeFile;
using SmartAttendance.Application.Base.MinIo.Requests.Commands.UploadFile;
using SmartAttendance.Application.Base.MinIo.Requests.Commands.UploadPdf;
using SmartAttendance.Application.Interfaces.HubFiles;
using SmartAttendance.Application.Interfaces.Minio;
using SmartAttendance.Common.General.Enums.FileType;
using SmartAttendance.Common.Utilities.EnumHelpers;
using SmartAttendance.Domain.HubFiles;
using WriterFactory = SharpCompress.Writers.WriterFactory;

namespace SmartAttendance.Persistence.Repositories.HubFiles;

public class HubFileCommandRepository(
    WriteOnlyDbContext                                     dbContext,
    ILogger<HubFileCommandRepository>                      logger,
    IStringLocalizer<HubFileCommandRepository>             localizer,
    IMultiTenantContextAccessor<SmartAttendanceTenantInfo> tenantContextAccessor,
    IMinIoCommandRepository                                minIoCommandRepository,
    IMinIoQueryRepository                                  minIoQueryRepository
) : CommandRepository<HubFile>(dbContext, logger),
    IHubFileCommandRepository
{
    public SmartAttendanceTenantInfo tenantContext => tenantContextAccessor.MultiTenantContext.TenantInfo!;


    public async Task<HubFile> PostFile(UploadFileRequest uploadFileRequest, CancellationToken cancellationToken)
    {
        logger.LogInformation("Uploading file to MinIO for tenant: {Tenant}", tenantContext.Identifier);
        var uploadFileCommand = uploadFileRequest.Adapt<UploadFileCommand>();

        try
        {
            var path = await minIoCommandRepository.UploadFileAsync(uploadFileCommand, cancellationToken);
            await AddAsync(path, cancellationToken);
            logger.LogInformation("File uploaded and saved successfully: {FilePath}", path.Path);
            return path;
        }
        catch (OutOfMemoryException)
        {
            logger.LogError("Out of memory while saving file for tenant: {Tenant}", tenantContext.Identifier);
            throw new Exception(localizer["Your storage quota has been exceeded."]);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled error occurred during file upload.");
            throw new Exception(localizer["An error occurred while uploading the file."]);
        }
    }

    public async Task<string> GetZipAsync(ZipExportCommand Zipfile, CancellationToken cancellationToken)
    {
        // logger.LogInformation("Generating ZIP file for ProjectId: {ProjectId}", Zipfile.ProjectId);

        var files = await TableNoTracking.Where(a =>
                                                    // a.ProjectId == Zipfile.ProjectId &&
                                                    a.ReferenceIdType == Zipfile.RowType  &&
                                                    a.ReportDate      >= Zipfile.FromDate &&
                                                    a.ReportDate      <= Zipfile.ToDate).
                                          GroupBy(a => a.Name).
                                          Select(a => a.OrderByDescending(w => w.ReportDate).FirstOrDefault()).
                                          ToListAsync(cancellationToken);

        if (files.Count == 0)
        {
            // logger.LogWarning("No files found to include in ZIP. ProjectId: {ProjectId}", Zipfile.ProjectId);
            // return null!;
        }

        var zip = await CreateZipFromFiles(files, cancellationToken);

        var dto = new ZipExportCommandResponse
        {
            File = zip,

            RowType    = FileStorageType.ZipExports,
            RowId      = Guid.NewGuid(),
            ReportDate = DateTime.UtcNow
        };

        var zipexport = dto.Adapt<UploadFileRequest>();
        var path      = await PostFile(zipexport, cancellationToken);

        DeleteFileJob(path.Path, 45);
        return GetUrlPath(path.Id, FileType.Zip, FileStorageType.ZipExports);
    }


    public async Task<string> SavePdfFile(
        UploadPdfCommand  file,
        CancellationToken cancellationToken)
    {
        var minioPath = await GetBucketPath(file.Adapt<UploadFileRequest>(), cancellationToken);

        var uploadPdfRequest = new UploadPdfRequest
        {
            File = file.File,

            FileName = file.FileName,
            RowId    = Guid.NewGuid(),
            RowType  = FileStorageType.PdfExports
        };

        var files = new UploadPdfCommand
        {
            File = file.File,

            FileName   = file.FileName,
            RowId      = Guid.NewGuid(),
            RowType    = FileStorageType.PdfExports,
            FileUpload = uploadPdfRequest,
            Path       = minioPath,
            UserId     = Guid.Empty
        };


        var path = await minIoCommandRepository.UploadPdfAsync(files, cancellationToken);


        try
        {
            await AddAsync(path, cancellationToken);
            logger.LogInformation("PDF file metadata saved successfully. FileId: {FileId}", path.Id);

            return GetUrlPath(path.Id, FileType.Pdf, FileStorageType.PdfExports);
        }
        catch (Exception ex)
        {
            // logger.LogError(ex, "Unhandled error while saving PDF file. ProjectId: {ProjectId}", file.ProjectId);
            throw new Exception(localizer["An error occurred while saving the PDF file."]);
        }
        finally
        {
            if (path != null)
            {
                DeleteFileJob(path.Path, 10);
                logger.LogInformation("Scheduled deletion for temporary PDF file: {Path}", path.Path);
            }
        }
    }

    // public async Task<SaveExcelCommandBrokerResponse> SaveExcelFile(
    //     SaveExcelCommandBroker file,
    //     CancellationToken cancellationToken)
    // {
    //     var minioPath = await GetBucketPath(file.Adapt<UploadFileRequest>(), cancellationToken);
    //
    //     var uploadExcelRequest = new UploadPdfRequest // reuse structure
    //     {
    //         File = file.File,

    //         FileName = file.FileName,
    //         RowId = Guid.NewGuid(),
    //         RowType = FileStorageType.ExcelExports
    //     };
    //
    //     var uploadCmd = new UploadPdfCommand // reuse command but for excel
    //     {
    //         File = file.File,

    //         FileName = file.FileName,
    //         RowId = Guid.NewGuid(),
    //         RowType = FileStorageType.ExcelExports,
    //         FileUpload = uploadExcelRequest,
    //         Path = minioPath,
    //         UserId = Guid.Empty
    //     };
    //
    //     var path = await minIoCommandRepository.UploadExcelAsync(uploadCmd, cancellationToken);
    //
    //     var storage = new StorageResponse(file.ProjectId,
    //         path.Path,
    //         path.Type,
    //         new decimal(file.File.Length / (1024.0 * 1024.0)));
    //
    //     var storages = storage.Adapt<Domain.Storages.Storage>();
    //
    //     if (file.RowType != FileStorageType.ZipExports &&
    //         file.RowType != FileStorageType.PdfExports &&
    //         file.RowType != FileStorageType.ExcelExports)
    //         await storageCommandRepository.AddAsync(storages, cancellationToken);
    //
    //     try
    //     {
    //         await AddAsync(path, cancellationToken);
    //         return new SaveExcelCommandBrokerResponse(GetUrlPath(path.Id,
    //             FileType.Excel,
    //             FileStorageType.ExcelExports));
    //     }
    //     finally
    //     {
    //         DeleteFileJob(path.Path, 10);
    //     }
    // }
    //
    // public async Task<SaveXmlCommandBrokerResponse> SaveXmlFile(
    //     SaveXmlCommandBroker file,
    //     CancellationToken cancellationToken)
    // {
    //     var minioPath = await GetBucketPath(file.Adapt<UploadFileRequest>(), cancellationToken);
    //
    //     var uploadXmlRequest = new UploadPdfRequest
    //     {
    //         File = file.File,

    //         FileName = file.FileName,
    //         RowId = Guid.NewGuid(),
    //         RowType = FileStorageType.MspExports
    //     };
    //
    //     var uploadCmd = new UploadPdfCommand
    //     {
    //         File = file.File,

    //         FileName = file.FileName,
    //         RowId = Guid.NewGuid(),
    //         RowType = FileStorageType.MspExports,
    //         FileUpload = uploadXmlRequest,
    //         Path = minioPath,
    //         UserId = Guid.Empty
    //     };
    //
    //     var path = await minIoCommandRepository.UploadXmlAsync(uploadCmd, cancellationToken);
    //
    //     var storage = new StorageResponse(file.ProjectId,
    //         path.Path,
    //         path.Type,
    //         new decimal(file.File.Length / (1024.0 * 1024.0)));
    //
    //     if (file.RowType != FileStorageType.ZipExports &&
    //         file.RowType != FileStorageType.PdfExports &&
    //         file.RowType != FileStorageType.ExcelExports &&
    //         file.RowType != FileStorageType.MspExports)
    //         await storageCommandRepository.AddAsync(storage.Adapt<Domain.Storages.Storage>(), cancellationToken);
    //
    //     await AddAsync(path, cancellationToken);
    //     return new SaveXmlCommandBrokerResponse(GetUrlPath(path.Id, FileType.Xml, FileStorageType.MspExports));
    // }


    private async Task<string> GetBucketPath(UploadFileRequest uploadFileRequest, CancellationToken cancellationToken)
    {
        var path = $"tenant-{tenantContext.Identifier}";

        if (uploadFileRequest.RowType is FileStorageType.CompanyLogo or FileStorageType.ProfilePicture
                                                                     or FileStorageType.CompanyMessage
                                                                     or FileStorageType.ZipExports or FileStorageType.PdfExports)
            return Path.Combine(path, uploadFileRequest.RowType.GetEnglishName()).ToLower();


        return Path.Combine(path, uploadFileRequest.RowType.GetEnglishName(), $"Date-{DateTime.UtcNow:yyyyMMdd}").ToLower();
    }

    private string GetUrlPath(Guid id, FileType fileType, FileStorageType storageType, bool compress = false)
    {
        return
            $"{tenantContext.Identifier}.{ApplicationConstant.Const.BaseDomain}/api/minio/hub-file/{id}?Type={fileType}&ReferenceType={storageType}&compress={compress}";
    }

    private async Task<IFormFile> CreateZipFromFiles(List<HubFile> filePaths, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating ZIP from {Count} files.", filePaths?.Count ?? 0);

        if (filePaths == null || !filePaths.Any())
            throw new ArgumentException(localizer["The list of files cannot be null or empty."]);

        byte[] zipBytes;

        using var zipStream = new MemoryStream();

        using (var writer = WriterFactory.Open(zipStream, ArchiveType.Zip, CompressionType.Deflate))
        {
            foreach (var hubFile in filePaths)
            {
                try
                {
                    await using var fileStream =
                        await minIoQueryRepository.GetFileAsync(hubFile.Path, cancellationToken);

                    if (fileStream.Length == 0)
                        continue;

                    writer.Write(hubFile.Name!, fileStream, DateTime.UtcNow);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error processing file during ZIP creation: {FileName}", hubFile.Name);
                }
            }
        }

        zipStream.Position = 0;
        var resultStream = new MemoryStream(zipStream.ToArray());

        var zipFile = new FormFile(resultStream,
                                   0,
                                   resultStream.Length,
                                   "file",
                                   $"report_{filePaths.FirstOrDefault()?.Type.ToString() ?? "Unknown"}_{DateTime.UtcNow:yyyy_MM_dd_HH_mm_ss}.zip")
        {
            Headers     = new HeaderDictionary(),
            ContentType = "application/zip"
        };

        logger.LogInformation("ZIP file created successfully.");
        return zipFile;
    }

    private void DeleteFileJob(string file, int removeTime)
    {
        try
        {
            // حذف زمان‌بندی‌شده در سیستم خارجی/کریون‌جاب
            logger.LogInformation("Scheduled deletion for file: {File}, in {Minutes} minutes.", file, removeTime);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error scheduling deletion for file: {File}", file);
        }
    }
}