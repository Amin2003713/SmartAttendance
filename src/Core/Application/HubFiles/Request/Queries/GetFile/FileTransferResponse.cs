namespace Shifty.Application.HubFiles.Request.Queries.GetFile;

public record FileTransferResponse(
    byte[] FileBytes,
    string FileName
);