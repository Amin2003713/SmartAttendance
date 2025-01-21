using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shifty.ApiFramework.Attributes;
using Shifty.ApiFramework.Tools;
using Shifty.Domain.Tenants;
using System;

namespace Shifty.Api.Controllers.Common
{
    [ValidateModelState]
    [Authorize]
    [Route("[controller]")]
    [ProducesResponseType(typeof(ApiProblemDetails) , StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiProblemDetails) , StatusCodes.Status500InternalServerError)]
    public class BaseController : ControllerBase
    {
        private IServiceProvider Resolver => HttpContext.RequestServices;

        protected IMediator Mediator => GetService<IMediator>();

        protected ILogger Logger => GetService<ILogger>();

        protected IMultiTenantContext<ShiftyTenantInfo> TenantContext => HttpContext.GetMultiTenantContext<ShiftyTenantInfo>();

        private T GetService<T>()
        {
            return Resolver.GetService<T>();
        }
    }
}