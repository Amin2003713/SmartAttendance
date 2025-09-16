namespace SmartAttendance.Domain.Common;

public readonly record struct RoleId(
    Guid Value
)
{
    public static   RoleId New()
    {
        return new RoleId(Guid.NewGuid());
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}