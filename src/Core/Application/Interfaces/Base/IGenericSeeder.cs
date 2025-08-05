using System.Threading;
using System.Threading.Tasks;

namespace Shifty.Application.Interfaces.Base;

public interface IGenericSeeder<in TDbContext>
{
    Task SeedAsync(TDbContext dbContext, CancellationToken cancellationToken);
}