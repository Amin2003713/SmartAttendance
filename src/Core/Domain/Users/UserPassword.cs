namespace Shifty.Domain.Users;

public class UserPassword : BaseEntity
{
    public string PasswordHash { get; set; } = null!;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}