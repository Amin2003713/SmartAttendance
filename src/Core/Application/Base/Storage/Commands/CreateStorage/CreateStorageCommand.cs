using SmartAttendance.Application.Base.Storage.Request.Commands.CreateStorage;
using SmartAttendance.Common.General.Enums.FileType;

namespace SmartAttendance.Application.Base.Storage.Commands.CreateStorage;

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