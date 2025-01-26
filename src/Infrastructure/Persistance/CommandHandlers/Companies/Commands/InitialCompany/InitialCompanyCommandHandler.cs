using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using Shifty.Application.Companies.Command.InitialCompany;
using Shifty.Common;
using Shifty.Common.Exceptions;
using Shifty.Domain.Interfaces.Companies;
using Shifty.Domain.Interfaces.Users;
using Shifty.Domain.Tenants;
using Shifty.Persistence.Services.MigrationManagers;
using Shifty.Resources.Messages;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shifty.Persistence.CommandHandlers.Companies.Commands.InitialCompany
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

            var adminUser = await CreateAdminUser(request , cancellationToken);

            var company = await InitialCompany(request , adminUser.Id , cancellationToken);


            var userCode = await MigrateDatabaseAsync(company , adminUser , cancellationToken);

            return userCode;
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
            catch (Exception e)
            {
                logger.LogError(e.Source , e);
                throw ShiftyException.InternalServerError(additionalData: commonMessages.Server_Error());
            }
        }


        private async Task<ShiftyTenantInfo> InitialCompany(InitialCompanyCommand request , Guid userId , CancellationToken cancellationToken)
        {
            try
            {
                var validation = await repository.ValidateDomain(request.Domain , cancellationToken);
                if (validation)
                    throw ShiftyException.BadRequest(additionalData: companyMessages.Tenant_Is_Not_Valid());

                var company = request.Adapt<ShiftyTenantInfo>();
                company.UserId = userId;

                var createResult = await repository.CreateAsync(company , cancellationToken);

                return createResult;
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
            catch (Exception e)
            {
                logger.LogError(e.Source , e);
                throw ShiftyException.InternalServerError(additionalData: commonMessages.Code_Generator() + commonMessages.Server_Error());
            }
        }


    }
}