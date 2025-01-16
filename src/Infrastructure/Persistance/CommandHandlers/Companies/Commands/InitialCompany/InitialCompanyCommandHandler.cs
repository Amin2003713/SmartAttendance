using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shifty.Application.Companies.Command.InitialCompany;
using Shifty.Common;
using Shifty.Common.Exceptions;
using Shifty.Domain.Interfaces.Companies;
using Shifty.Domain.Interfaces.Users;
using Shifty.Domain.Tenants;
using Shifty.Persistence.Services.MigrationManagers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shifty.Persistence.CommandHandlers.Companies.Commands.InitialCompany
{
    public class InitialCompanyCommandHandler(
        ICompanyRepository repository
        , ITenantAdminRepository tenantAdminRepository
        , RunTimeDatabaseMigrationService runTimeDatabaseMigrationService)
        : IRequestHandler<InitialCompanyCommand , IActionResult>
    {
        public async Task<IActionResult> Handle(InitialCompanyCommand request , CancellationToken cancellationToken)
        {
            if (request is null)
                throw new InvalidNullInputException(nameof(request));


            var company = await InitialCompany(request , cancellationToken);

            var adminUser = await CreateAdminUser(request  , company , cancellationToken);

            if (!await MigrateDatabaseAsync(company , adminUser , cancellationToken))
                throw new ShiftyException(ApiResultStatusCode.DataBaseError , "Can't migrate database");

            return new CreatedResult();
        }

        private async Task<TenantAdmin> CreateAdminUser(InitialCompanyCommand request , ShiftyTenantInfo company , CancellationToken cancellationToken)
        {
            var tenantAdmin = await tenantAdminRepository.CreateAsync(request.Adapt<TenantAdmin>() , company , cancellationToken);

            if (tenantAdmin == null)
                throw new ShiftyException(ApiResultStatusCode.DataBaseError , "Can't create Admin User");

            return tenantAdmin;
        }


        private async Task<ShiftyTenantInfo> InitialCompany(InitialCompanyCommand request , CancellationToken cancellationToken)
        {
            try
            {
                var validation = await repository.ValidateDomain(request.Domain , cancellationToken);
                if (!validation.IsValid)
                    throw new ShiftyException(ApiResultStatusCode.BadRequest , validation.message);

                var company = request.Adapt<ShiftyTenantInfo>();

                var createResult = await repository.CreateAsync(company , cancellationToken , false);

                if (createResult is null)
                    throw new ShiftyException(ApiResultStatusCode.DataBaseError , "Can't create company Server error");

                return createResult;
            }
            catch (Exception e)
            {
                throw new ShiftyException(ApiResultStatusCode.DataBaseError , $"Can't create company Server error {e}");
            }
        }

        private async Task<bool> MigrateDatabaseAsync(ShiftyTenantInfo company , TenantAdmin adminUser , CancellationToken cancellationToken)
        {
            var connectionString = company.GetConnectionString();
            return await runTimeDatabaseMigrationService.MigrateTenantDatabasesAsync(connectionString , adminUser , cancellationToken);
        }
    }
}