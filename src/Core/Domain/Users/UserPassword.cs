using SmartAttendance.Common.General.BaseClasses;

namespace SmartAttendance.Domain.Users;

public class UserPassword : BaseEntity
{
    public string PasswordHash { get; set; } = null!;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}