using MediatR;
using Shifty.Application.Tenants.Command;
using System;

namespace Shifty.Application.Tenants.Requests
{
    public class CreateTenantRequest : IRequest<CreateTenantResponse>
    {
        public string Identifier { get; set; }

        public string Name { get; set; }

        public Guid UserId { get; set; }
    }
}