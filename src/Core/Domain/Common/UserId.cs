namespace SmartAttendance.Domain.Common;

// شناسه‌های قوی-تایپ به صورت readonly record struct
public readonly record struct UserId(
    Guid Value
)
{
    public static UserId New()
    {
        return new UserId(Guid.NewGuid());
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}