namespace SmartAttendance.Domain.Common;

// رویداد دامنه

// کلاس پایه Entity با شناسه جنریک
public abstract class Entity<TId>(
    TId id
)
{
    public TId Id { get; init; } = id;

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj)) return true;
        if (obj is not Entity<TId> other) return false;

        return EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }

    public override int GetHashCode()
    {
        return EqualityComparer<TId>.Default.GetHashCode(Id!);
    }
}

// کلاس پایه AggregateRoot با مدیریت رویدادهای دامنه