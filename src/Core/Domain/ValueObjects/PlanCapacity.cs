using SmartAttendance.Domain.Common;

namespace SmartAttendance.Domain.ValueObjects;

// VO: ظرفیت طرح با قوانین حداقل/حداکثر و رزرو
public sealed class PlanCapacity
{
	public int Capacity { get; }
	public int Reserved { get; private set; }

	public PlanCapacity(int capacity, int reserved = 0)
	{
		if (capacity <= 0) throw new DomainValidationException("ظرفیت باید بیشتر از صفر باشد.");
		if (reserved < 0) throw new DomainValidationException("رزرو نمی‌تواند منفی باشد.");
		if (reserved > capacity) throw new DomainValidationException("رزرو از ظرفیت بیشتر است.");
		Capacity = capacity;
		Reserved = reserved;
	}

	public bool IsFull => Reserved >= Capacity;

	public PlanCapacity ReserveOne()
	{
		if (IsFull) throw new BusinessRuleViolationException("ظرفیت طرح تکمیل شده است.");
		return new PlanCapacity(Capacity, Reserved + 1);
	}

	public PlanCapacity ReleaseOne()
	{
		if (Reserved == 0) return this;
		return new PlanCapacity(Capacity, Reserved - 1);
	}

	public override string ToString() => $"{Reserved}/{Capacity}";
}

