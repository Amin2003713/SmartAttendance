namespace SmartAttendance.Domain.ValueObjects;

// VO: وضعیت حضور
public enum AttendanceStatus
{
    Unknown       = 0,
    Present       = 1,
    Absent        = 2,
    Late          = 3,
    Excused       = 4,
    Manual        = 5,
    OfflineSynced = 6
}