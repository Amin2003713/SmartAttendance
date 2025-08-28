using Shifty.Common.Common.Responses.GetLogPropertyInfo.OperatorLogs;

namespace Shifty.Common.Common.Responses.Users.Queries.Base;

public class GetUserResponse
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string UserName { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string PhoneNumber { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string FirstName { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string LastName { get; set; }

    public Guid Id { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Profile { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ProfileCompress { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Address { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string> Roles { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTime? LastActionOnServer { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? JobTitle { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTime? BirthDate { get; set; }

    public bool IsActive { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public LogPropertyInfoResponse? CreatedBy { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime CreatedAt { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]

    public string? Email { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]

    public string FullName { get; set; }
}