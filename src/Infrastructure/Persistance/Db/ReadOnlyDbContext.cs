using Microsoft.EntityFrameworkCore;

namespace Shifty.Persistence.Db
{
    public class ReadOnlyDbContext(DbContextOptions<AppDbContext> options) : AppDbContext(options);
}