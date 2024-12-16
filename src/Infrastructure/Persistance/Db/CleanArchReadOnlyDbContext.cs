using Microsoft.EntityFrameworkCore;

namespace Shifty.Persistence.Db
{
    public class CleanArchReadOnlyDbContext(DbContextOptions<AppDbContext> options) : AppDbContext(options);
}
