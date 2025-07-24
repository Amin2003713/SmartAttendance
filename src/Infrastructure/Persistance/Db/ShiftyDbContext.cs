using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Shifty.Common.Utilities.EfCoreHelper;
using Users_Role = Shifty.Domain.Users.Role;

namespace Shifty.Persistence.Services.DbContexts;

public abstract class ShiftyDbContext :
    IdentityDbContext<User, Users_Role, Guid, IdentityUserClaim<Guid>, UserRoles,
        IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>,
    IAppDbContext
{
    private readonly IdentityService? _identityService;

    public ShiftyDbContext(
        DbContextOptions<ShiftyDbContext> options,
        IdentityService currentUserId)
        : base(options)
    {
        _identityService = currentUserId;
    }

    public ShiftyDbContext(DbContextOptions<ShiftyDbContext> options)
        : base(options) { }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyEntityRules();
        return base.SaveChangesAsync(cancellationToken);
    }

    public Task<int> ExecuteSqlRawAsync(string query, CancellationToken cancellationToken)
    {
        return Database.ExecuteSqlRawAsync(query, cancellationToken);
    }

    public Task<int> ExecuteSqlRawAsync(string query)
    {
        return ExecuteSqlRawAsync(query, CancellationToken.None);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var entitiesAssembly = typeof(User).Assembly;


        modelBuilder.RegisterAllEntities(entitiesAssembly);
        modelBuilder.ApplyConfigurationsFromAssembly(entitiesAssembly);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetCallingAssembly());
        modelBuilder.AddPluralizingTableNameConvention();
        modelBuilder.AddDecimalConvention();
        modelBuilder.AddGlobalIsActiveFilter();

        modelBuilder.AddFeatureBasedSchema(entitiesAssembly.GetType());

        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        ApplyEntityRules();
        return base.SaveChanges();
    }

    private void ApplyEntityRules()
    {
        if (_identityService is null) return;

        var entries       = ChangeTracker.Entries<IEntity>();
        var currentUserId = _identityService.GetUserId();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy ??= currentUserId ?? null!;
                    entry.Entity.IsActive = true;
                    break;

                case EntityState.Modified:
                    entry.Entity.ModifiedAt = DateTime.Now;
                    entry.Entity.ModifiedBy ??= currentUserId ?? null!;
                    break;

                case EntityState.Deleted:
                    entry.Entity.DeletedAt = DateTime.Now;
                    entry.Entity.DeletedBy ??= currentUserId ?? null!;
                    entry.Entity.IsActive = false;
                    entry.State = EntityState.Modified;
                    break;
            }
        }
    }
}