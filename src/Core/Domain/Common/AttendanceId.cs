namespace SmartAttendance.Domain.Common;

public readonly record struct AttendanceId(
    Guid Value
)
{
    public static   AttendanceId New()
    {
        return new AttendanceId(Guid.NewGuid());
    }

    public override string       ToString()
    {
        return Value.ToString();
    }
}