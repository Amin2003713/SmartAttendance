using Microsoft.EntityFrameworkCore;

namespace Shifty.Persistence.Db
{
    public class WriteOnlyDbContext(DbContextOptions<AppDbContext> options) : AppDbContext(options);
}