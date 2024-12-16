using Shifty.Api.Controllers.v1.Users.Requests;
using Shifty.ApiFramework.Tools;
using Shifty.Application.Users.Command.Login;
using Shifty.Application.Users.Command.RefreshToken;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shifty.Application.Users.Command.CreateUser.Admin;
using Shifty.Domain.Entities.Users;
using Shifty.Domain.Enums;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace Shifty.Api.Controllers.v1.Users
{
    [ApiVersion("1")]
    public class UserController : BaseControllerV1
    {
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
