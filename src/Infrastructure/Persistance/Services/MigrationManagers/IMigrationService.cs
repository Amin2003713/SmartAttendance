using System.Threading.Tasks;

namespace Shifty.Api.Services
{
    public interface IMigrationService
    {
        Task ApplyMigrations();
    }
}