namespace Shifty.Domain.Time.ir;

public class ContactInfo
{
    [JsonProperty("name")] public string Name { get; set; }

    [JsonProperty("url")] public string Url { get; set; }

    [JsonProperty("email")] public string Email { get; set; }
}