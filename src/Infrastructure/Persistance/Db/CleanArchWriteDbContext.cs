using Microsoft.EntityFrameworkCore;

namespace Shifty.Persistence.Db
{
    public class CleanArchWriteDbContext(DbContextOptions<AppDbContext> options) : AppDbContext(options);
}