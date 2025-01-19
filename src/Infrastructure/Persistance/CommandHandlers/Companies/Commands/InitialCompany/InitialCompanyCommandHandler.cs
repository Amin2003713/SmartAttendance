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
        : IRequestHandler<InitialCompanyCommand , string>
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
            var tenantAdmin = await tenantAdminRepository.CreateAsync(request.Adapt<TenantAdmin>() , cancellationToken);

            if (tenantAdmin == null)
                throw new ShiftyException(ApiResultStatusCode.DataBaseError , "Can't create Admin User");

            return tenantAdmin;
        }


        private async Task<ShiftyTenantInfo> InitialCompany(InitialCompanyCommand request , Guid userId , CancellationToken cancellationToken)
        {
            try
            {
                var validation = await repository.ValidateDomain(request.Domain , cancellationToken);
                if (!validation.IsValid)
                    throw new ShiftyException(ApiResultStatusCode.BadRequest , validation.message);

                var company = request.Adapt<ShiftyTenantInfo>();
                company.UserId = userId;

                var createResult = await repository.CreateAsync(company , cancellationToken);

                if (createResult is null)
                    throw new ShiftyException(ApiResultStatusCode.DataBaseError , "Can't create company Server error");

                return createResult;
            }
            catch (Exception e)
            {
                throw new ShiftyException(ApiResultStatusCode.DataBaseError , $"Can't create company Server error {e}");
            }
        }

        private async Task<string> MigrateDatabaseAsync(ShiftyTenantInfo company , TenantAdmin adminUser , CancellationToken cancellationToken)
        {
            return await runTimeDatabaseMigrationService.MigrateTenantDatabasesAsync(company.GetConnectionString() , adminUser , cancellationToken) ??
                   throw new ShiftyException(ApiResultStatusCode.DataBaseError , "Activation code sent error ");
        }
    }
}