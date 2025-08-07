using Mapster;
using Shifty.Application.Companies.Commands.InitialCompany;
using Shifty.Application.Interfaces.Tenants.Companies;
using Shifty.Application.Interfaces.Tenants.Users;
using Shifty.Common.Exceptions;
using Shifty.Domain.Tenants;
using Shifty.Persistence.Db;
using Shifty.Persistence.Services.RunTimeServiceSetup;

namespace Shifty.RequestHandlers.Companies.Commands.InitialCompany;

public class InitialCompanyCommandHandler(
    ICompanyRepository repository,
    ITenantAdminRepository tenantAdminRepository,
    RunTimeDatabaseMigrationService runTimeDatabaseMigrationService,
    IStringLocalizer<InitialCompanyCommandHandler> localizer,
    ILogger<InitialCompanyCommandHandler> logger,
    ShiftyTenantDbContext dbContext
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

            var userCode = await MigrateDatabaseAsync(company, request.NationalCode, adminUser, cancellationToken);

            await transaction.CommitAsync(cancellationToken);


            return userCode;
        }
        catch (IpaException e)
        {
            await transaction.RollbackAsync(cancellationToken);
            logger.LogError(e, "DRP Exception occurred.");
            throw;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);
            logger.LogError(e, "Unexpected error occurred.");
            throw new IpaException(localizer["Unexpected server error occurred."].Value);
        }
    }

    private async Task<TenantAdmin> CreateAdminUser(InitialCompanyCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var tenantAdmin = await tenantAdminRepository.CreateAsync(request.Adapt<TenantAdmin>(), cancellationToken);
            if (tenantAdmin == null)
                throw IpaException.InternalServerError(
                    additionalData: localizer["Company admin was not created."].Value);

            return tenantAdmin;
        }
        catch (IpaException e)
        {
            logger.LogError(e, "Error while creating admin user.");
            throw;
        }
    }

    private async Task<ShiftyTenantInfo> InitialCompany(
        InitialCompanyCommand request,
        Guid userId,
        CancellationToken cancellationToken)
    {
        try
        {
            if (await repository.ValidateDomain(request.Domain, cancellationToken))
                throw IpaException.BadRequest(additionalData: localizer["Tenant domain is not valid."].Value);

            var company = request.Adapt<ShiftyTenantInfo>();
            company.UserId = userId;
            var createResult = await repository.CreateAsync(company, cancellationToken);

            return createResult;
        }
        catch (IpaException e)
        {
            logger.LogError(e, "Error while initializing company.");
            throw;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unexpected error while creating company.");
            throw new IpaException(localizer["Company was not created."].Value);
        }
    }

    private async Task<string> MigrateDatabaseAsync(
        ShiftyTenantInfo company,
        string password,
        TenantAdmin adminUser,
        CancellationToken cancellationToken)
    {
        try
        {
            return await runTimeDatabaseMigrationService.MigrateTenantDatabasesAsync(company,
                password,
                adminUser,
                cancellationToken);
        }
        catch (IpaException e)
        {
            logger.LogError(e, "Error while migrating tenant database.");
            throw new IpaException(localizer["Company was not created."].Value);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unexpected server error during database migration.");
            throw IpaException.InternalServerError(additionalData: localizer["Unexpected server error occurred."]
                .Value);
        }
    }
}