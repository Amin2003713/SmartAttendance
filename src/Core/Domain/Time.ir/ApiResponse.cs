namespace Shifty.Domain.Time.ir;

public class ApiResponse
{
    [JsonProperty("status_code")] public int StatusCode { get; set; }

    [JsonProperty("object_type")] public string ObjectType { get; set; }

    [JsonProperty("time_taken")] public int TimeTaken { get; set; }

    [JsonProperty("creation_date")] public DateTime CreationDate { get; set; }

    [JsonProperty("url")] public string Url { get; set; }

    [JsonProperty("message")] public string Message { get; set; }

    [JsonProperty("error")] public object Error { get; set; }

    [JsonProperty("data")] public ApiData Data { get; set; }

    [JsonProperty("meta")] public ApiMeta Meta { get; set; }
}