using System;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shifty.ApiFramework.Attributes;
using Shifty.ApiFramework.Tools;
using Shifty.Domain.Tenants;
using Shifty.Persistence.Services.Identities;

namespace Shifty.ApiFramework.Controller;

[ValidateModelState]
[Authorize]
[Route("api/[controller]")]
[ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status500InternalServerError)]
[ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status401Unauthorized)]
public class ShiftyBaseController : ControllerBase
{
    private IServiceProvider Resolver => HttpContext.RequestServices;

    protected IMediator Mediator => GetService<IMediator>();
    protected IdentityService IdentityService => GetService<IdentityService>();

    protected IMultiTenantContext<ShiftyTenantInfo> TenantContext => HttpContext.GetMultiTenantContext<ShiftyTenantInfo>();

    public T GetService<T>()
    {
        return Resolver.GetService<T>()!;
    }
}