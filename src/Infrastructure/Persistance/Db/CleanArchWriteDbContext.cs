using Microsoft.EntityFrameworkCore;
using Shifty.Persistence.TenantServices;

namespace Shifty.Persistence.Db
{
    public class WriteOnlyDbContext(DbContextOptions<AppDbContext> options) : AppDbContext(options);
}