namespace SmartAttendance.Application.Features.Users.Requests.Queries.UserInfo;

public class UserInfoResponse
{
    public string? Username { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Profile { get; set; } = null!;
    public string? Address { get; set; } = null!;
    public DateTime? LastActionOnServer { get; set; } = null!;
    public string? JobTitle { get; set; }
}