using Finbuckle.MultiTenant.Abstractions;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shifty.ApiFramework.Tools;
using Shifty.Application.Companies.Requests;
using Shifty.Application.Tenants.Command;
using Shifty.Domain.Tenants;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace Shifty.Api.Controllers.v1.Tenants;

[ApiVersion("1")]
public class CompanyController(IMultiTenantContextAccessor<ShiftyTenantInfo> accessor) : BaseControllerV1
{

    [HttpPost("AdminsPanel/CreateCompany")]
    [SwaggerOperation("Create a new tenant.")]
    [Authorize]
    public virtual async Task<ApiResult<CreateCompanyResponse>> CreateCompany([FromBody] CreateCompanyRequest request , CancellationToken cancellationToken)
    {
        var command = request.Adapt<CreateCompanyCommand>();
    
        var result = await Mediator.Send(command, cancellationToken);
        return new ApiResult<CreateCompanyResponse>(result);
    }
}