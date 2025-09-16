using SmartAttendance.Domain.Common;

namespace SmartAttendance.Domain.UserAggregate;

public sealed class Role(
    RoleId id,
    string name
) : Entity<RoleId>(id)
{
    public string Name { get; } = string.IsNullOrWhiteSpace(name)
        ? throw new DomainValidationException("نام نقش الزامی است.")
        : name.Trim();
}