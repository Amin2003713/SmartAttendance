using Finbuckle.MultiTenant.Abstractions;
using Shifty.ApiFramework.Tools;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shifty.Application.Users.Command.CreateUser.Admin;
using Shifty.Application.Users.Command.CreateUser.Employee;
using Shifty.Application.Users.Command.Login;
using Shifty.Application.Users.Command.RefreshToken;
using Shifty.Application.Users.Requests;
using Shifty.Domain.Enums;
using Shifty.Domain.Tenants;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace Shifty.Api.Controllers.v1.Users
{
    [ApiVersion("1")]
    public class UserController(IMultiTenantContextAccessor<ShiftyTenantInfo> accessor) : BaseControllerV1
    {

        [HttpPost("Test")]
        [SwaggerOperation("Test ")]
        [AllowAnonymous]
        public virtual async Task<ApiResult<string>> test( CancellationToken cancellationToken)
        {
            return new ApiResult<string>($"{accessor.MultiTenantContext.TenantInfo?.Identifier}");
        }

        [HttpPost("Admins/sign-up")]
        [SwaggerOperation("sign up user")]
        [AllowAnonymous]
        public virtual async Task<ApiResult<bool>> SingUpAsync([FromBody] SingUpAdminRequest request,
            CancellationToken cancellationToken)
        {
            var command = request.Adapt<RegisterAdminCommand>();

            var result = await Mediator.Send(command, cancellationToken);
            return new ApiResult<bool>(result);
        }

        [HttpPost("Employee/sign-up")]
        [SwaggerOperation("sign up user")]
        [Authorize(Roles = nameof(UserRoles.Admin) )]
        public virtual async Task<ApiResult<bool>> SingUpEmployeeAsync([FromBody] SingUpEmployeeRequest request, CancellationToken cancellationToken)
        {
            var command = request.Adapt<RegisterEmployeeCommand>();

            var result = await Mediator.Send(command, cancellationToken);
            return new ApiResult<bool>(result);
        }

        [HttpPost("login")]
        [SwaggerOperation("login by username and password")]
        [AllowAnonymous]
        public virtual async Task<ApiResult<LoginResponse>> LoginAsync([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            var command = request.Adapt<LoginCommand>();

            var result = await Mediator.Send(command, cancellationToken);
            return new ApiResult<LoginResponse>(result);
        }

        [HttpPost("refreshToken")]
        [SwaggerOperation("get new refresh and access token")]
        [AllowAnonymous]
        public virtual async Task<ApiResult<RefreshTokenResponse>> RefreshTokenAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var command = request.Adapt<RefreshTokenCommand>();

            var result = await Mediator.Send(command, cancellationToken);
            return new ApiResult<RefreshTokenResponse>(result);
        }
    }
}
