using System.Web;
using Microsoft.AspNetCore.Http;
using SmartAttendance.Application.Base.HubFiles.Commands.AddFileForCopyRow;
using SmartAttendance.Application.Base.HubFiles.Commands.UploadHubFile;
using SmartAttendance.Application.Interfaces.HubFiles;
using SmartAttendance.Application.Interfaces.Minio;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Common.General;
using SmartAttendance.Common.General.Enums.FileType;

namespace SmartAttendance.RequestHandlers.Base.HubFiles.Commands.AddFileForCopyRow;

public record UploadFileForCopyRowCommandHandler(
    IHubFileQueryRepository                              HubFileQueryRepository,
    IMinIoCommandRepository                              MinIoCommandRepository,
    IMediator                                            Mediator,
    ILogger<UploadFileForCopyRowCommandHandler>          Logger,
    IStringLocalizer<UploadFileForCopyRowCommandHandler> Localizer
)
    : IRequestHandler<UploadFileForCopyRowCommand, MediaFileStorage>
{
    public async Task<MediaFileStorage> Handle(UploadFileForCopyRowCommand request, CancellationToken cancellationToken)
    {
        // Logger.LogInformation("Handling UploadFileForCopyRowCommand for ProjectId {ProjectId}.", request.ProjectId);

        if (string.IsNullOrEmpty(request.FileUrl))
        {
            Logger.LogWarning("FileUrl is null or empty.");
            return null!;
        }

        (Guid rowId, FileType fileType, FileStorageType fileStorageType) requestParams;

        try
        {
            requestParams = ExtractUrlParameters(request.FileUrl);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to extract URL parameters from FileUrl {FileUrl}.", request.FileUrl);
            throw SmartAttendanceException.BadRequest(Localizer["Invalid FileUrl."]);
        }

        var fileInfo = await HubFileQueryRepository.GetHubFile(
            requestParams.rowId,
            requestParams.fileType,
            requestParams.fileStorageType,
            cancellationToken);

        if (fileInfo == null)
        {
            Logger.LogWarning(
                "File information not found for RowId {RowId}, FileType {FileType}, StorageType {FileStorageType}.",
                requestParams.rowId,
                requestParams.fileType,
                requestParams.fileStorageType);

            throw SmartAttendanceException.NotFound(Localizer["File information is null."]);
        }

        try
        {
            var file = ToFormFile(fileInfo.FileBytes, $"{DateTimeOffset.Now.ToFileTime()}_Copied_{fileInfo.FileName}");
            // Logger.LogInformation("Uploading file for ProjectId {ProjectId} with RowId {RowId}.",
            //     request.ProjectId,
            //     request.RowId);

            var uploadResult = await Mediator.Send(new UploadHubFileCommand
                {
                    File       = file!,
                    ReportDate = request.ReportDate,
                    RowId      = request.RowId,
                    RowType    = request.FileStorageType
                },
                cancellationToken);

            Logger.LogInformation("File uploaded successfully. Upload result: {UploadResult}", uploadResult);
            return uploadResult;
        }
        catch (Exception ex)
        {
            // Logger.LogError(ex,
            //     "An error occurred while uploading the file for ProjectId {ProjectId}.",
            //     request.ProjectId);

            throw SmartAttendanceException.InternalServerError(Localizer["An error occurred while uploading the file."]);
        }
    }

    /// <summary>
    ///     Extracts URL parameters from the provided URL.
    ///     Returns a tuple containing the RowId, FileType, and FileStorageType.
    /// </summary>
    private(Guid rowId, FileType fileType, FileStorageType fileStorageType) ExtractUrlParameters(string url)
    {
        var uri   = new Uri(url);
        var rowId = Guid.Parse(uri.Segments.Last());

        var queryParams = HttpUtility.ParseQueryString(uri.Query);

        if (!Enum.TryParse<FileType>(queryParams["Type"], true, out var fileType))
            throw new ArgumentException("Invalid FileType in URL parameters.");

        if (!Enum.TryParse<FileStorageType>(queryParams["ReferenceType"], true, out var referenceType))
            throw new ArgumentException("Invalid FileStorageType in URL parameters.");

        return (rowId, fileType, referenceType);
    }

    /// <summary>
    ///     Converts a byte array into an IFormFile.
    /// </summary>
    public static IFormFile ToFormFile(byte[] bytes, string fileName, string contentType = "application/octet-stream")
    {
        var stream = new MemoryStream(bytes);

        var formFile = new FormFile(stream, 0, bytes.Length, "file", fileName)
        {
            Headers     = new HeaderDictionary(),
            ContentType = contentType
        };

        return formFile;
    }
}