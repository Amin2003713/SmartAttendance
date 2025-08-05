namespace Shifty.Application.MinIo.Commands.DeleteFile;

public record DeleteFileCommand(
    string FilePath
) : IRequest<bool>;