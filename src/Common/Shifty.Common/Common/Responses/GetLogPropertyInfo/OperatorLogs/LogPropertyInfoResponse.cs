namespace Shifty.Common.Common.Responses.GetLogPropertyInfo.OperatorLogs;

public class LogPropertyInfoResponse
{
    public Guid Id { get; set; }
    public string PhoneNumber { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Profile { get; set; }

    public string FullName => $"{FirstName} {LastName}";
}