namespace Shifty.Domain.Users;

public class Role : IdentityRole<Guid>,
    ISimpleEntity
{
    public string Description { get; set; }
    public bool IsActive { get; set; } = true;
}