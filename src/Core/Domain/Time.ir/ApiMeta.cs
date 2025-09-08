namespace SmartAttendance.Domain.Time.ir;

public class ApiMeta
{
    [JsonProperty("title")] public string Title { get; set; }

    [JsonProperty("description")] public string Description { get; set; }

    [JsonProperty("term_of_use")] public string TermOfUse { get; set; }

    [JsonProperty("copyright")] public string Copyright { get; set; }

    [JsonProperty("version")] public string Version { get; set; }

    [JsonProperty("contact")] public ContactInfo Contact { get; set; }

    [JsonProperty("license")] public LicenseInfo License { get; set; }
}