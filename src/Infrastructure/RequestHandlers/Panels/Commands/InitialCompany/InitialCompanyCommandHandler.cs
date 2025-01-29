using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using Shifty.Application.Panel.Companies.Command.InitialCompany;
using Shifty.Common.Exceptions;
using Shifty.Domain.Interfaces.Tenants.Companies;
using Shifty.Domain.Interfaces.Tenants.Users;
using Shifty.Domain.Tenants;
using Shifty.Persistence.Services.MigrationManagers;
using Shifty.Resources.Messages;

namespace Shifty.RequestHandlers.Panels.Commands.InitialCompany
{
    public class InitialCompanyCommandHandler(
        ICompanyRepository repository ,
        ITenantAdminRepository tenantAdminRepository ,
        RunTimeDatabaseMigrationService runTimeDatabaseMigrationService ,
        CompanyMessages companyMessages ,
        CommonMessages commonMessages ,
        ILogger<InitialCompanyCommandHandler> logger) : IRequestHandler<InitialCompanyCommand , string>
    {
        public async Task<string> Handle(InitialCompanyCommand request , CancellationToken cancellationToken)
        {
            if (request is null)
                throw new InvalidNullInputException(nameof(request));

            try
            {
                var adminUser = await CreateAdminUser(request , cancellationToken);

                var company = await InitialCompany(request , adminUser.Id , cancellationToken);


                var userCode = await MigrateDatabaseAsync(company , adminUser , cancellationToken);

                return userCode;
            }
            catch (ShiftyException e)
            {
                logger.LogError(e.Source , e);
                throw;
            }
        }

        private async Task<TenantAdmin> CreateAdminUser(InitialCompanyCommand request , CancellationToken cancellationToken)
        {
            try
            {
                var tenantAdmin = await tenantAdminRepository.CreateAsync(request.Adapt<TenantAdmin>() , cancellationToken);

                if (tenantAdmin == null)
                    throw ShiftyException.InternalServerError(additionalData: companyMessages.Company_Admin_Not_Created());

                return tenantAdmin;
            }
            catch (ShiftyException e)
            {
                logger.LogError(e.Source , e);
                throw;
            }
        }


        private async Task<ShiftyTenantInfo> InitialCompany(InitialCompanyCommand request , Guid userId , CancellationToken cancellationToken)
        {
            try
            {
                if (await repository.ValidateDomain(request.Domain , cancellationToken))
                    throw ShiftyException.BadRequest(additionalData: companyMessages.Tenant_Is_Not_Valid());

                var company = request.Adapt<ShiftyTenantInfo>();
                company.UserId = userId;

                var createResult = await repository.CreateAsync(company , cancellationToken);

                return createResult;
            }
            catch (ShiftyException e)
            {
                logger.LogError(e.Source , e);
                throw;
            }
            catch (Exception e)                                         
            {
                logger.LogError(e.Source , e);
                throw new ShiftyException(companyMessages.Company_Not_Created());
            }
        }

        private async Task<string> MigrateDatabaseAsync(ShiftyTenantInfo company , TenantAdmin adminUser , CancellationToken cancellationToken)
        {
            try
            {
                return await runTimeDatabaseMigrationService.MigrateTenantDatabasesAsync(company.GetConnectionString() , adminUser , cancellationToken);
            }
            catch (ShiftyException e)
            {
                logger.LogError(e.Source , e);
                throw;
            }
            catch (Exception e)
            {
                logger.LogError(e.Source , e);
                throw ShiftyException.InternalServerError(additionalData: commonMessages.Code_Generator() + commonMessages.Server_Error());
            }
        }


    }
}