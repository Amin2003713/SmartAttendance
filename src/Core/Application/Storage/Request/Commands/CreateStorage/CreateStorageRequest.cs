using Shifty.Common.General.Enums.FileType;

namespace Shifty.Application.Storage.Request.Commands.CreateStorage;

public class CreateStorageRequest
{
    public Guid RowId { get; set; }

    public FileType FileType { get; set; }
    public decimal StorageUsedByItemMb { get; set; }
}