namespace SmartAttendance.Common.Common.Responses.GetLogPropertyInfo.OperatorLogs;

public class LogPropertyInfoResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Profile { get; set; }
    public string? Compressed { get; set; }
    public string UserName { get; set; }
}