using SmartAttendance.Common.General.BaseClasses;

namespace SmartAttendance.Domain.Users;

public class UserToken : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid UniqueId { get; set; }
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
    public DateTime ExpiryTime { get; set; }
}