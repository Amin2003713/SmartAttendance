namespace SmartAttendance.Application.Features.Attendance.Requests;

// درخواست تایید معذوریت
public sealed class ApproveExcuseRequest
{
	public string Reason { get; init; } = string.Empty;
}

