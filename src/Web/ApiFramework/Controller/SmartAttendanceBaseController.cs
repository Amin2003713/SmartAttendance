using System;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SmartAttendance.ApiFramework.Attributes;
using SmartAttendance.ApiFramework.Tools;
using SmartAttendance.Domain.Tenants;
using SmartAttendance.Persistence.Services.Identities;

namespace SmartAttendance.ApiFramework.Controller;

[ValidateModelState]
[Authorize]
[Route("api/[controller]")]
[ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status500InternalServerError)]
[ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status401Unauthorized)]
public class SmartAttendanceBaseController : ControllerBase
{
    private IServiceProvider Resolver => HttpContext.RequestServices;

    protected IMediator Mediator => GetService<IMediator>();
    protected IdentityService IdentityService => GetService<IdentityService>();

    protected IMultiTenantContext<SmartAttendanceTenantInfo> TenantContext => HttpContext.GetMultiTenantContext<SmartAttendanceTenantInfo>();

    public T GetService<T>()
    {
        return Resolver.GetService<T>()!;
    }
}