using Microsoft.EntityFrameworkCore;
using Shifty.Persistence.Db;
using Shifty.Persistence.TenantServices;

namespace Shifty.Persistence.Services
{
    public class AppDbContextFactory : DesignTimeDbContextFactoryBase<AppDbContext>
    {
        public AppDbContextFactory()
        {
            
        }
        public AppDbContextFactory(ITenantService services)
        {
        }

        protected override AppDbContext CreateNewInstance(DbContextOptions<AppDbContext> options ) =>
            new AppDbContext();
    }
}
