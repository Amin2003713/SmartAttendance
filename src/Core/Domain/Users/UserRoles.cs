using Shifty.Common.General.BaseClasses;

namespace Shifty.Domain.Users;

public class UserRoles : IdentityUserRole<Guid>,
    ISimpleEntity
{
    public Guid Id { get; set; } = Guid.CreateVersion7(DateTimeOffset.Now);
}