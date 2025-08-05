using Shifty.Common.General.BaseClasses;
using Shifty.Common.General.Enums.FileType;

namespace Shifty.Domain.Storages;

public class Storage : BaseEntity
{
    // public Guid? ProjectId { get; set; }

    public FileType FileType { get; set; }

    public Guid RowId { get; set; }
    public decimal StorageUsedByItemMb { get; set; }
}