namespace SmartAttendance.Persistence.Db;

public class WriteOnlyDbContext(
    DbContextOptions<SmartAttendanceDbContext> options,
    IdentityService userId
)
    : SmartAttendanceDbContext(options, userId);