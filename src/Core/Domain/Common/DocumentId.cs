namespace SmartAttendance.Domain.Common;

public readonly record struct DocumentId(
    Guid Value
)
{
    public static   DocumentId New()
    {
        return new DocumentId(Guid.NewGuid());
    }

    public override string     ToString()
    {
        return Value.ToString();
    }
}