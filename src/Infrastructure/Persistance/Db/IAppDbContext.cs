namespace Shifty.Persistence.Services.DbContexts;

public interface IAppDbContext
{
    DbSet<T> Set<T>()
        where T : class;

    Task<int> SaveChangesAsync(CancellationToken cancellation);
    Task<int> ExecuteSqlRawAsync(string query, CancellationToken cancellationToken);
    Task<int> ExecuteSqlRawAsync(string query);
}