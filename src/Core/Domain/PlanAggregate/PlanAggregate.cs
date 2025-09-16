using SmartAttendance.Domain.Common;
using SmartAttendance.Domain.Events;
using SmartAttendance.Domain.ValueObjects;

namespace SmartAttendance.Domain.PlanAggregate;

// Aggregate Root: طرح با ظرفیت، ثبت‌نام، لیست انتظار
public sealed class PlanAggregate : AggregateRoot<PlanId>
{
	private readonly HashSet<UserId> _students = new();
	private readonly Queue<UserId> _waitingList = new();

	public string Title { get; private set; }
	public string Description { get; private set; }
	public DateTime StartsAtUtc { get; private set; }
	public DateTime EndsAtUtc { get; private set; }
	public PlanCapacity Capacity { get; private set; }

	public IReadOnlyCollection<UserId> Students => _students;
	public IReadOnlyCollection<UserId> WaitingList => _waitingList.ToList().AsReadOnly();

	public PlanAggregate(PlanId id, string title, string description, DateTime startsAtUtc, DateTime endsAtUtc, PlanCapacity capacity)
		: base(id)
	{
		Title = Normalize(title, "عنوان");
		Description = (description ?? string.Empty).Trim();
		if (endsAtUtc <= startsAtUtc) throw new DomainValidationException("تاریخ پایان باید بعد از شروع باشد.");
		StartsAtUtc = startsAtUtc;
		EndsAtUtc = endsAtUtc;
		Capacity = capacity ?? throw new DomainValidationException("ظرفیت الزامی است.");
	}

	private static string Normalize(string value, string field)
	{
		value = value?.Trim() ?? throw new DomainValidationException($"{field} الزامی است.");
		if (value.Length is < 3 or > 200) throw new DomainValidationException($"{field} نامعتبر است.");
		return value;
	}

	public void RegisterStudent(UserId studentId)
	{
		if (_students.Contains(studentId)) return;

		if (!Capacity.IsFull)
		{
			Capacity = Capacity.ReserveOne();
			_students.Add(studentId);
			RaiseDomainEvent(new StudentRegisteredToPlanEvent(Id, studentId));

			if (Capacity.IsFull)
			{
				RaiseDomainEvent(new PlanCapacityReachedEvent(Id));
			}
		}
		else
		{
			if (_waitingList.Contains(studentId)) return;
			_waitingList.Enqueue(studentId);
		}
	}

	public void CancelEnrollment(UserId studentId)
	{
		if (_students.Remove(studentId))
		{
			Capacity = Capacity.ReleaseOne();
			RaiseDomainEvent(new EnrollmentCanceledEvent(Id, studentId));

			if (_waitingList.Count > 0 && !Capacity.IsFull)
			{
				var next = _waitingList.Dequeue();
				RegisterStudent(next);
			}
		}
		else
		{
			if (_waitingList.Count > 0)
			{
				var remaining = new Queue<UserId>();
				while (_waitingList.Count > 0)
				{
					var s = _waitingList.Dequeue();
					if (!EqualityComparer<UserId>.Default.Equals(s, studentId))
						remaining.Enqueue(s);
				}
				while (remaining.Count > 0) _waitingList.Enqueue(remaining.Dequeue());
			}
		}
	}

	public void Reschedule(DateTime startsAtUtc, DateTime endsAtUtc)
	{
		if (endsAtUtc <= startsAtUtc) throw new DomainValidationException("تاریخ پایان باید بعد از شروع باشد.");
		StartsAtUtc = startsAtUtc;
		EndsAtUtc = endsAtUtc;
	}
}

