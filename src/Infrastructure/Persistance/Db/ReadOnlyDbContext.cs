namespace SmartAttendance.Persistence.Db;

public class ReadOnlyDbContext(
    DbContextOptions<SmartAttendanceDbContext> options,
    IdentityService userId
)
    : SmartAttendanceDbContext(options, userId);