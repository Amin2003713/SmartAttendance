namespace SmartAttendance.Persistence.Mongo.Documents;

// مدل سندی کاربر برای MongoDB
public sealed class UserDocument
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string NationalCode { get; set; } = string.Empty;
    public int FailedLoginCount { get; set; }
    public bool IsLocked { get; set; }
    public DateTime? LockedAtUtc { get; set; }
    public List<Guid> RoleIds { get; set; } = new();
}