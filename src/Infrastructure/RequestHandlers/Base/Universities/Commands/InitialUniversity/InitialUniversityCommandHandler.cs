using Mapster;
using SmartAttendance.Application.Base.Universities.Commands.InitialUniversity;
using SmartAttendance.Application.Interfaces.Tenants.Companies;
using SmartAttendance.Application.Interfaces.Tenants.Users;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Domain.Tenants;
using SmartAttendance.Persistence.Db;
using SmartAttendance.Persistence.Services.RunTimeServiceSetup;

namespace SmartAttendance.RequestHandlers.Base.Universites.Commands.InitialUniversity;

public class InitialUniversityCommandHandler(
    IUniversityRepository                             repository,
    IUniversityAdminRepository                         UniversityAdminRepository,
    RunTimeDatabaseMigrationService                runTimeDatabaseMigrationService,
    IStringLocalizer<InitialUniversityCommandHandler> localizer,
    ILogger<InitialUniversityCommandHandler>          logger,
    SmartAttendanceTenantDbContext                 dbContext
)
    : IRequestHandler<InitialUniversityCommand, string>
{
    public async Task<string> Handle(InitialUniversityCommand request, CancellationToken cancellationToken)
    {
        if (request is null)
            throw new InvalidNullInputException(nameof(request));

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var adminUser  = await CreateAdminUser(request, cancellationToken);
            var University = await InitialUniversity(request, adminUser.Id, cancellationToken);

            var userCode = await MigrateDatabaseAsync(University, request.Password, adminUser, cancellationToken);

            await transaction.CommitAsync(cancellationToken);


            return userCode;
        }
        catch (SmartAttendanceException e)
        {
            await transaction.RollbackAsync(cancellationToken);
            logger.LogError(e, "Sm Exception occurred.");
            throw;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);
            logger.LogError(e, "Unexpected error occurred.");
            throw new SmartAttendanceException(localizer["Unexpected server error occurred."].Value);
        }
    }

    private async Task<UniversityAdmin> CreateAdminUser(InitialUniversityCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var UniversityAdmin = await UniversityAdminRepository.CreateAsync(request.Adapt<UniversityAdmin>(), cancellationToken);

            if (UniversityAdmin == null)
                throw SmartAttendanceException.InternalServerError(
                    additionalData: localizer["University admin was not created."].Value);

            return UniversityAdmin;
        }
        catch (SmartAttendanceException e)
        {
            logger.LogError(e, "Error while creating admin user.");
            throw;
        }
    }

    private async Task<UniversityTenantInfo> InitialUniversity(
        InitialUniversityCommand request,
        Guid                  userId,
        CancellationToken     cancellationToken)
    {
        try
        {
            if (await repository.ValidateDomain(request.Domain, cancellationToken))
                throw SmartAttendanceException.BadRequest(additionalData: localizer["Tenant domain is not valid."].Value);

            var University = request.Adapt<UniversityTenantInfo>();
            University.BranchAdminId = userId;
            var createResult = await repository.CreateAsync(University, cancellationToken);

            return createResult;
        }
        catch (SmartAttendanceException e)
        {
            logger.LogError(e, "Error while initializing University.");
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unexpected error while creating University.");
            throw new SmartAttendanceException(localizer["University was not created."].Value);
        }
    }

    private async Task<string> MigrateDatabaseAsync(
        UniversityTenantInfo University,
        string                    password,
        UniversityAdmin               adminUser,
        CancellationToken         cancellationToken)
    {
        try
        {
            return await runTimeDatabaseMigrationService.MigrateTenantDatabasesAsync(University,
                password,
                adminUser,
                cancellationToken);
        }
        catch (SmartAttendanceException e)
        {
            logger.LogError(e, "Error while migrating tenant database.");
            throw new SmartAttendanceException(localizer["University was not created."].Value);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unexpected server error during database migration.");
            throw SmartAttendanceException.InternalServerError(additionalData: localizer["Unexpected server error occurred."].Value);
        }
    }
}