namespace SmartAttendance.Domain.Common;

// رویداد دامنه
public interface IDomainEvent
{
	DateTime OccurredOnUtc { get; }
}

// کلاس پایه Entity با شناسه جنریک
public abstract class Entity<TId>(TId id)
{
	public TId Id { get; init; } = id;

	public override bool Equals(object? obj)
	{
		if (ReferenceEquals(this, obj)) return true;
		if (obj is not Entity<TId> other) return false;
		return EqualityComparer<TId>.Default.Equals(Id, other.Id);
	}
	public override int GetHashCode() => EqualityComparer<TId>.Default.GetHashCode(Id!);
}

// کلاس پایه AggregateRoot با مدیریت رویدادهای دامنه
public abstract class AggregateRoot<TId>(TId id) : Entity<TId>(id)
{
	private readonly List<IDomainEvent> _domainEvents = new();

	public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

	protected void RaiseDomainEvent(IDomainEvent domainEvent)
	{
		_domainEvents.Add(domainEvent);
	}

	public void ClearDomainEvents() => _domainEvents.Clear();
}

