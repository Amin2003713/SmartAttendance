namespace SmartAttendance.Common.General;

public class MediaFileStorage
{
    public string Url { get; set; }
    public FileType Type { get; set; }
    public string? FileName { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Compressed { get; set; }
}