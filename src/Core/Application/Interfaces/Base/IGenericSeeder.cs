using System.Threading;
using System.Threading.Tasks;

namespace SmartAttendance.Application.Interfaces.Base;

public interface IGenericSeeder<in TDbContext>
{
    Task SeedAsync(TDbContext dbContext, CancellationToken cancellationToken);
}