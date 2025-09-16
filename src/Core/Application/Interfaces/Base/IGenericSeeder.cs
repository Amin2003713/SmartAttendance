namespace SmartAttendance.Application.Interfaces.Base;

public interface IGenericSeeder<in TDbContext>
{
    Task SeedAsync(TDbContext dbContext, CancellationToken cancellationToken);
}