namespace SmartAttendance.Application.Base.HubFiles.Request.Commands.ZipExport;

public class ZipExportCommandResponse
{
    public IFormFile File { get; set; }

    // public Guid? ProjectId { get; set; }
    public Guid RowId { get; set; }
    public DateTime ReportDate { get; set; }
    public FileStorageType RowType { get; set; }


    public string FileExtension()
    {
        return (Path.GetExtension(File?.FileName) ?? string.Empty).Replace(".", "");
    }
}