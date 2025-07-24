namespace Shifty.Common.Common.Responses.Media;

public class MediaResponse
{
    public string Url { get; set; }
    public FileType Type { get; set; }
    public string? FileName { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Compressed { get; set; }
}