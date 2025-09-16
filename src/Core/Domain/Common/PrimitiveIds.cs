namespace SmartAttendance.Domain.Common;

// شناسه‌های قوی-تایپ به صورت readonly record struct
public readonly record struct UserId(Guid Value)
{
	public static UserId New() => new(Guid.NewGuid());
	public override string ToString() => Value.ToString();
}

public readonly record struct RoleId(Guid Value)
{
	public static RoleId New() => new(Guid.NewGuid());
	public override string ToString() => Value.ToString();
}

public readonly record struct PlanId(Guid Value)
{
	public static PlanId New() => new(Guid.NewGuid());
	public override string ToString() => Value.ToString();
}

public readonly record struct AttendanceId(Guid Value)
{
	public static AttendanceId New() => new(Guid.NewGuid());
	public override string ToString() => Value.ToString();
}

public readonly record struct DocumentId(Guid Value)
{
	public static DocumentId New() => new(Guid.NewGuid());
	public override string ToString() => Value.ToString();
}

public readonly record struct NotificationId(Guid Value)
{
	public static NotificationId New() => new(Guid.NewGuid());
	public override string ToString() => Value.ToString();
}

