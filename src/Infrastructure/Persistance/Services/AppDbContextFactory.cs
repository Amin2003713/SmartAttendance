using Microsoft.EntityFrameworkCore;
using Shifty.Persistence.Db;

namespace Shifty.Persistence.Services
{
    public class AppDbContextFactory : DesignTimeDbContextFactoryBase<AppDbContext>
    {
        public AppDbContextFactory()
        { }

        protected override AppDbContext CreateNewInstance(DbContextOptions<AppDbContext> options)
        {
            return new AppDbContext(options);
        }
    }
}
