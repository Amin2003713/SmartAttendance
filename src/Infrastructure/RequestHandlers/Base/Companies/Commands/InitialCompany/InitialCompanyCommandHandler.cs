using Mapster;
using SmartAttendance.Application.Base.Companies.Commands.InitialCompany;
using SmartAttendance.Application.Interfaces.Tenants.Companies;
using SmartAttendance.Application.Interfaces.Tenants.Users;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Domain.Tenants;
using SmartAttendance.Persistence.Db;
using SmartAttendance.Persistence.Services.RunTimeServiceSetup;

namespace SmartAttendance.RequestHandlers.Base.Companies.Commands.InitialCompany;

public class InitialCompanyCommandHandler(
    ICompanyRepository                             repository,
    ITenantAdminRepository                         tenantAdminRepository,
    RunTimeDatabaseMigrationService                runTimeDatabaseMigrationService,
    IStringLocalizer<InitialCompanyCommandHandler> localizer,
    ILogger<InitialCompanyCommandHandler>          logger,
    SmartAttendanceTenantDbContext                 dbContext
)
    : IRequestHandler<InitialCompanyCommand, string>
{
    public async Task<string> Handle(InitialCompanyCommand request, CancellationToken cancellationToken)
    {
        if (request is null)
            throw new InvalidNullInputException(nameof(request));

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var adminUser = await CreateAdminUser(request, cancellationToken);
            var company   = await InitialCompany(request, adminUser.Id, cancellationToken);

            var userCode = await MigrateDatabaseAsync(company, request.Password, adminUser, cancellationToken);

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

    private async Task<TenantAdmin> CreateAdminUser(InitialCompanyCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var tenantAdmin = await tenantAdminRepository.CreateAsync(request.Adapt<TenantAdmin>(), cancellationToken);

            if (tenantAdmin == null)
                throw SmartAttendanceException.InternalServerError(
                    additionalData: localizer["Company admin was not created."].Value);

            return tenantAdmin;
        }
        catch (SmartAttendanceException e)
        {
            logger.LogError(e, "Error while creating admin user.");
            throw;
        }
    }

    private async Task<SmartAttendanceTenantInfo> InitialCompany(
        InitialCompanyCommand request,
        Guid                  userId,
        CancellationToken     cancellationToken)
    {
        try
        {
            if (await repository.ValidateDomain(request.Domain, cancellationToken))
                throw SmartAttendanceException.BadRequest(additionalData: localizer["Tenant domain is not valid."].Value);

            var company = request.Adapt<SmartAttendanceTenantInfo>();
            company.UserId = userId;
            var createResult = await repository.CreateAsync(company, cancellationToken);

            return createResult;
        }
        catch (SmartAttendanceException e)
        {
            logger.LogError(e, "Error while initializing company.");
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unexpected error while creating company.");
            throw new SmartAttendanceException(localizer["Company was not created."].Value);
        }
    }

    private async Task<string> MigrateDatabaseAsync(
        SmartAttendanceTenantInfo company,
        string                    password,
        TenantAdmin               adminUser,
        CancellationToken         cancellationToken)
    {
        try
        {
            return await runTimeDatabaseMigrationService.MigrateTenantDatabasesAsync(company,
                password,
                adminUser,
                cancellationToken);
        }
        catch (SmartAttendanceException e)
        {
            logger.LogError(e, "Error while migrating tenant database.");
            throw new SmartAttendanceException(localizer["Company was not created."].Value);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unexpected server error during database migration.");
            throw SmartAttendanceException.InternalServerError(additionalData: localizer["Unexpected server error occurred."].Value);
        }
    }
}