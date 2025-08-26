using Shifty.Common.General.Enums.FileType;

namespace Shifty.Application.Base.Storage.Commands.UpdateStorage;

public class UpdateStorageCommand : IRequest
{
    public Guid Id { get; set; }

    // public Guid? ProjectId { get; set; }

    public FileType FileType { get; set; }

    public Guid RowId { get; set; }

    public decimal StorageUsedByItemMb { get; set; }
}