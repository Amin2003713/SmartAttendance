using SmartAttendance.Common.General.BaseClasses;
using SmartAttendance.Common.General.Enums.FileType;

namespace SmartAttendance.Domain.Storages;

public class Storage : BaseEntity
{
    // public Guid? ProjectId { get; set; }

    public FileType FileType { get; set; }

    public Guid RowId { get; set; }
    public decimal StorageUsedByItemMb { get; set; }
}