using System.Threading.Tasks;

namespace Shifty.Persistence.Services.MigrationManagers
{
    public interface IMigrationService
    {
        Task ApplyMigrations();
    }
}