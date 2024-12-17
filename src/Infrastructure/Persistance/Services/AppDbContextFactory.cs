using Microsoft.EntityFrameworkCore;
using Shifty.Persistence.Db;
using Shifty.Persistence.TenantServices;

namespace Shifty.Persistence.Services
{
    public class AppDbContextFactory(ITenantService services) : DesignTimeDbContextFactoryBase<AppDbContext>
    {
        protected override AppDbContext CreateNewInstance(DbContextOptions<AppDbContext> options ) =>
            new AppDbContext(options , services);
    }
}
