namespace SmartAttendance.Domain.Common;

public readonly record struct PlanId(
    Guid Value
)
{
    public static   PlanId New()
    {
        return new PlanId(Guid.NewGuid());
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}