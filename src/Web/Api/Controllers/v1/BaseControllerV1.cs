using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Shifty.Domain.Tenants;

namespace Shifty.Api.Controllers.v1
{
    using ApiFramework.Attributes;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using System;

    [ValidateModelState]
    [Authorize]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BaseControllerV1() : ControllerBase
    {
  

        private IServiceProvider Resolver => HttpContext.RequestServices;

        private T GetService<T>()
        {
            return Resolver.GetService<T>();
        }

        protected IMediator Mediator => GetService<IMediator>();

        protected ILogger Logger => GetService<ILogger>();

        protected IMultiTenantContext<ShiftyTenantInfo> TenantContext => HttpContext.GetMultiTenantContext<ShiftyTenantInfo>();
    }
}