namespace Shifty.Persistence.Db;

public class ReadOnlyDbContext(
    DbContextOptions<ShiftyDbContext> options,
    IdentityService userId
)
    : ShiftyDbContext(options, userId);