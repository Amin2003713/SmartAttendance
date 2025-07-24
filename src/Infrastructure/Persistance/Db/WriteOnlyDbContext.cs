namespace Shifty.Persistence.Db;

public class WriteOnlyDbContext(
    DbContextOptions<ShiftyDbContext> options,
    IdentityService userId
)
    : ShiftyDbContext(options, userId);