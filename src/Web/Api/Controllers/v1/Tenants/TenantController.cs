using Finbuckle.MultiTenant.Abstractions;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shifty.ApiFramework.Tools;

using Shifty.Domain.Tenants;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace Shifty.Api.Controllers.v1.Tenants;

[ApiVersion("1")]
public class TenantController(IMultiTenantContextAccessor<ShiftyTenantInfo> accessor) : BaseControllerV1
{

    // [HttpPost("CreateTenant")]
    // [SwaggerOperation("Create a new tenant.")]
    // [AllowAnonymous]
    // public virtual async Task<ApiResult<CreateTenantResponse>> CreateTenant(CreateTenantRequest request , CancellationToken cancellationToken)
    // {
    //     var command = request.Adapt<CreateTenantCommand>();
    //
    //     var result = await Mediator.Send(command, cancellationToken);
    //     return new ApiResult<CreateTenantResponse>(result);
    // }
}