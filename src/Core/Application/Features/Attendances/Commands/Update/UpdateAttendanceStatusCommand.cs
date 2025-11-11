using SmartAttendance.Common.General.Enums.Attendance;

public class UpdateAttendanceStatusCommand : IRequest
{
    public Guid AttendanceId { get; set; }
    public AttendanceStatus Status { get; set; }
    public Guid? ExcuseId { get; set; } = null;
}