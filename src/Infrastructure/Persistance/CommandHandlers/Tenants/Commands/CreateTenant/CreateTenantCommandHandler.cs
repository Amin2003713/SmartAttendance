using MediatR;
using Shifty.Application.Tenants.Command;
using Shifty.Persistence.Db;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shifty.Persistence.CommandHandlers.Tenants.Commands.CreateTenant;

public class CreateTenantCommandHandler(TenantDbContext context)
    : IRequestHandler<CreateTenantCommand, CreateTenantResponse>
{
    public Task<CreateTenantResponse> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        return default;
    }
}