using Shifty.Common.General.BaseClasses;
using Shifty.Common.General.Enums.FileType;

namespace Shifty.Domain.HubFiles;

public class HubFile : BaseEntity
{
    public string? Name { get; set; }
    public required string Path { get; set; }

    public FileType Type { get; set; }

    public Guid ReferenceId { get; set; }
    public FileStorageType ReferenceIdType { get; set; }
    public DateTime ReportDate { get; set; }
}