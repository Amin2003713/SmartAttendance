using Shifty.Application.Base.Storage.Request.Commands.CreateStorage;
using Shifty.Common.General.Enums.FileType;

namespace Shifty.Application.Base.Storage.Commands.CreateStorage;

public class CreateStorageCommand : CreateStorageRequest,
    IRequest
{
    public CreateStorageCommand AddFileSize(double bytesToMegabytes, FileType fileType)
    {
        StorageUsedByItemMb = (decimal)bytesToMegabytes;
        FileType = fileType;
        return this;
    }
}