namespace SmartAttendance.Application.Base.HubFiles.Request.Queries.GetFile;

public record FileTransferResponse(
    byte[] FileBytes,
    string FileName
);