using Microsoft.EntityFrameworkCore;
using Shifty.Persistence.TenantServices;

namespace Shifty.Persistence.Db
{
    public class ReadOnlyDbContext(DbContextOptions<AppDbContext> options, ITenantService tenantService) : AppDbContext(options, tenantService);
}
