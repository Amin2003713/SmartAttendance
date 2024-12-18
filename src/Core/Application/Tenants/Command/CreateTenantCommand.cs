using MediatR;
using System;

namespace Shifty.Application.Tenants.Command;

public class CreateTenantCommand : IRequest<CreateTenantResponse>
{
    public string Identifier { get; set; }

    public string Name { get; set; }

    public Guid UserId { get; set; }
}