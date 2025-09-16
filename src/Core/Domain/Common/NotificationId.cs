namespace SmartAttendance.Domain.Common;

public readonly record struct NotificationId(
    Guid Value
)
{
    public static   NotificationId New()
    {
        return new NotificationId(Guid.NewGuid());
    }

    public override string         ToString()
    {
        return Value.ToString();
    }
}