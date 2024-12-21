using Mapster;
using MediatR;
using Shifty.Application.Tenants.Command;
using Shifty.Common;
using Shifty.Common.Exceptions;
using Shifty.Domain.Interfaces.Companies;
using Shifty.Domain.Interfaces.Users;
using Shifty.Domain.Tenants;
using Shifty.Persistence.Db;
using Shifty.Persistence.Services.MigrationManagers;
using System.Threading;
using System.Threading.Tasks;

namespace Shifty.Persistence.CommandHandlers.Companies.Commands.CreateCompany;

public class CreateCompanyCommandHandler(ICompanyRepository repository  , ITenantAdminRepository tenantAdminRepository, RunTimeDatabaseMigrationService runTimeDatabaseMigrationService)
    : IRequestHandler<CreateCompanyCommand, CreateCompanyResponse>
{
    public async Task<CreateCompanyResponse> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        if (request is null)
            throw new InvalidNullInputException(nameof(request));

        var company = request.Adapt<ShiftyTenantInfo>();
        

        if(!await repository.CanCreateAsync(company , cancellationToken))
            throw new ShiftyException(ApiResultStatusCode.BadRequest , "Can't create company Become its Already Exists");

        var createResult = await repository.CreateAsync(company, cancellationToken);

        if(!createResult)
            throw new ShiftyException(ApiResultStatusCode.ServerError , "Can't create company Server error");


        if (!await MigrateDatabaseAsync(company ,await tenantAdminRepository.GetByIdeAsync(request.UserId , cancellationToken) , cancellationToken))
            throw new ShiftyException(ApiResultStatusCode.ServerError, "Can't migrate database");



        return company.Adapt<CreateCompanyResponse>();
    }

    private async Task<bool> MigrateDatabaseAsync(ShiftyTenantInfo company, TenantAdmin adminUser , CancellationToken cancellationToken)
    {
        var connectionString = company.GetConnectionString();
        return await runTimeDatabaseMigrationService.MigrateTenantDatabasesAsync(connectionString , adminUser , cancellationToken);
    }
}


