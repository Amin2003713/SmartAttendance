using SmartAttendance.Application.Interfaces.Tenants.Users;
using SmartAttendance.Common.Utilities.InjectionHelpers;

// Added for logging

namespace SmartAttendance.Persistence.Repositories.Tenants.TenantAdmin;

public class UniversityAdminRepository : IUniversityAdminRepository,
    IScopedDependency
{
    private readonly SmartAttendanceTenantDbContext     _dbContext;
    private readonly ILogger<UniversityAdminRepository> _logger;

    // Constructor with dependency injection for DbContext and Logger
    public UniversityAdminRepository(SmartAttendanceTenantDbContext dbContext, ILogger<UniversityAdminRepository> logger)
    {
        _dbContext = dbContext;
        _logger    = logger;
    }

    private DbSet<UniversityAdmin> Entities => _dbContext.UniversityAdmins;
    protected virtual IQueryable<UniversityAdmin> Table => Entities;
    protected virtual IQueryable<UniversityAdmin> TableNoTracking => Entities.AsNoTracking();

    /// <summary>
    ///     Checks if a user exists based on the provided phone number.
    /// </summary>
    /// <param name="phoneNumber">The phone number to check.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if the user exists; otherwise, false.</returns>
    public async Task<bool> UserExists(string phoneNumber, CancellationToken cancellationToken)
    {
        return await TableNoTracking.AnyAsync(u => u.PhoneNumber == phoneNumber, cancellationToken);
    }

    /// <summary>
    ///     Creates a new UniversityAdmin user.
    /// </summary>
    /// <param name="user">The UniversityAdmin user to create.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created UniversityAdmin user.</returns>
    public async Task<UniversityAdmin> CreateAsync(
        UniversityAdmin user,
        CancellationToken          cancellationToken)
    {
        try
        {
            var existingUser = await GetByUniversityOrPhoneNumberAsync(user.PhoneNumber, cancellationToken);
            if (existingUser != null) return existingUser;

            Entities.Add(user);
            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("UniversityAdmin user with phone number: {PhoneNumber} created successfully.",
                user.PhoneNumber);

            return user;
        }
        catch (SmartAttendanceException)
        {
            // Re-throw custom exceptions without additional logging
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Source, ex);
            throw SmartAttendanceException.InternalServerError();
        }
    }

    /// <summary>
    ///     Retrieves a UniversityAdmin user by phone number. Throws an exception if not found.
    /// </summary>
    /// <param name="phoneNumber">The phone number of the user to retrieve.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The UniversityAdmin user.</returns>
    public async Task<UniversityAdmin> GetByUniversityOrPhoneNumberAsync(
        string            phoneNumber,
        CancellationToken cancellationToken)
    {
        try
        {
            return await GetByUniversityOrPhoneNumber(phoneNumber, cancellationToken);

            _logger.LogInformation(
                "Ipa.Models.Tenants.UniversityAdmin user with phone number: {PhoneNumber} retrieved successfully.",
                phoneNumber);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Source, ex);
            throw SmartAttendanceException.InternalServerError();
        }
    }

    /// <summary>
    ///     Private method to retrieve a Ipa.Models.Tenants.UniversityAdmin user by phone number without throwing exceptions.
    /// </summary>
    /// <param name="phoneNumber">The phone number of the user to retrieve.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The Ipa.Models.Tenants.UniversityAdmin user if found; otherwise, null.</returns>
    private async Task<UniversityAdmin> GetByUniversityOrPhoneNumber(
        string            phoneNumber,
        CancellationToken cancellationToken)
    {
        return (await Table.SingleOrDefaultAsync(a => a.PhoneNumber == phoneNumber, cancellationToken))!;
    }
}