using System.Net;
using SmartAttendance.Application.Base.HubFiles.Commands.UploadHubFile;
using SmartAttendance.Application.Base.HubFiles.Commands.ZipExport;
using SmartAttendance.Application.Base.HubFiles.Queries.GetHubFile;
using SmartAttendance.Application.Base.HubFiles.Request.Commands.UploadHubFile;
using SmartAttendance.Common.General.Enums.FileType;
using SmartAttendance.Common.Utilities.TypeConverters;

namespace SmartAttendance.Api.Controllers.HubFiles;

public class Hub_FileController : SmartAttendanceBaseController
{
    /// <summary>
    ///     Retrieves a file by its ID with optional compression.
    /// </summary>
    /// <param name="fileId">Unique identifier of the file.</param>
    /// <param name="type">The type of the file (e.g., PDF, Picture, Zip).</param>
    /// <param name="referenceType">The reference type related to the file (e.g., project, user, etc.).</param>
    /// <param name="compress">If true, the file will be compressed before sending.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>Returns the file as a downloadable attachment.</returns>
    /// <response code="200">Successfully retrieved the file.</response>
    /// <response code="400">Invalid parameters or request format.</response>
    /// <response code="401">Unauthorized access to file retrieval.</response>
    [HttpGet("{fileId:guid}")]
    [AllowAnonymous]
// #if DEBUG
//     [AllowAnonymous]
// #endif
    [SwaggerOperation(Summary = "Get HubFile",
        Description = "Fetches a file based on its ID and returns it as an attachment.")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<FileContentResult> GetHubFile(
        Guid fileId,
        FileType type,
        FileStorageType referenceType,
        bool compress,
        CancellationToken cancellationToken)
    {
        var contentType = type switch
                          {
                              FileType.Pdf => "application/pdf", FileType.Picture => "image/png", FileType.Zip => "application/zip",
                              _            => "application/octet-stream"
                          };

        var file = await Mediator.Send(GetHubFileQuery.Create(fileId, type, referenceType, compress),
            cancellationToken);

        Response.Headers.Append("Content-Disposition", $"attachment; filename={WebUtility.UrlEncode(file.FileName)}");
        return File(await file.FileBytes.CompressAsync(compress), contentType);
    }

    /// <summary>
    ///     Uploads a new file to the server.
    /// </summary>
    /// <param name="request">The request containing file metadata and content.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="201">File uploaded successfully.</response>
    /// <response code="400">Invalid file content or metadata.</response>
    /// <response code="401">Unauthorized to perform file upload.</response>
    [HttpPost]
    [SwaggerOperation(Summary = "Upload File", Description = "Uploads a new file to the hub.")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task UploadFile([FromBody] UploadHubFileRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request.Adapt<UploadHubFileCommand>(), cancellationToken);
    }

    /// <summary>
    ///     Exports selected files as a compressed ZIP archive.
    /// </summary>
    /// <param name="request">The request specifying files and metadata for export.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="201">ZIP file generated successfully.</response>
    /// <response code="400">Invalid request format or file selection.</response>
    /// <response code="401">Unauthorized to perform file export.</response>
    [HttpPost("zip-export")]
    [SwaggerOperation(Summary = "Zip Export", Description = "Exports selected files as a ZIP archive.")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task ZipExport([FromBody] ZipExportCommand request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request.Adapt<ZipExportCommand>(), cancellationToken);
    }
}