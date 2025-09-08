namespace SmartAttendance.Application.Base.MinIo.Commands.DeleteFile;

public record DeleteFileCommand(
    string FilePath
) : IRequest<bool>;