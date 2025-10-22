using SmartAttendance.Common.General.BaseClasses;
using SmartAttendance.Common.General.Enums.FileType;

namespace SmartAttendance.Domain.HubFiles;

public class HubFile : BaseEntity
{
    public string? Name { get; set; }
    public required string Path { get; set; }

    public FileType Type { get; set; }

    public Guid ReferenceId { get; set; }
}