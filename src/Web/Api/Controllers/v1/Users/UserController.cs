using Shifty.ApiFramework.Tools;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shifty.Application.Users.Command.CreateUser.Admin;
using Shifty.Application.Users.Command.CreateUser.Employee;
using Shifty.Application.Users.Command.Login;
using Shifty.Application.Users.Command.RefreshToken;
using Shifty.Application.Users.Command.SendActivationCode;
using Shifty.Application.Users.Requests;
using Shifty.Application.Users.Requests.Login;
using Shifty.Application.Users.Requests.SendActivationCode;
using Shifty.Application.Users.Requests.SingUp;
using Shifty.Domain.Enums;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace Shifty.Api.Controllers.v1.Users
{
    [ApiVersion("1")]
    public class UserController() : BaseControllerV1
    {
        /// <summary>
        /// Authenticates an admin user by username and password.
        /// </summary>
        /// <param name="request">The request containing the admin user's login credentials.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The response containing the authentication result and user details.</returns>
        /// <response code="200">Returns the authentication result and user details.</response>
        /// <response code="400">If the request is invalid (e.g., missing required fields or invalid credentials).</response>
        /// <response code="401">If the provided credentials are invalid or the user is not authorized.</response>
        /// <response code="500">If an internal server error occurs during authentication.</response>
        [HttpPost("AdminsPanel/login")]
        [SwaggerOperation("Authenticate an admin user by username and password.")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResponse) ,      StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResult) ,   StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResult) , StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiProblemDetails) ,     StatusCodes.Status500InternalServerError)]
        public virtual async Task<LoginResponse> AdminsLoginAsync([FromBody] LoginRequest request , CancellationToken cancellationToken) =>
            await Mediator.Send(request.Adapt<AdminsLoginCommand>() , cancellationToken);


        /// <summary>
        /// Sends an activation code to the specified user.
        /// </summary>
        /// <param name="request">The request containing the user's unique identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The response containing the status of the activation code sending process.</returns>
        /// <response code="200">Returns the status of the activation code sending process.</response>
        /// <response code="400">If the request is invalid (e.g., missing or invalid user ID).</response>
        /// <response code="404">If the user with the specified ID is not found.</response>
        /// <response code="500">If an internal server error occurs while sending the activation code.</response>
        [HttpPost("Admins/Send_Activation_code")]
        [SwaggerOperation("Send an activation code to the specified user.")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(SendActivationCodeResponse) , StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResult) ,           StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(NotFoundResult) ,             StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiProblemDetails) ,             StatusCodes.Status500InternalServerError)]
        public virtual async Task<SendActivationCodeResponse> SendActivationCode(
            [FromBody] SendActivationCodeRequest request
            , CancellationToken cancellationToken) =>
            await Mediator.Send(request.Adapt<SendActivationCodeCommand>() , cancellationToken);


        /// <summary>
        /// Registers a new employee user.
        /// </summary>
        /// <param name="request">The request containing the employee's details.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if the registration was successful; otherwise, false.</returns>
        /// <response code="200">Returns true if the registration was successful.</response>
        /// <response code="400">If the request is invalid (e.g., missing required fields or invalid data).</response>
        /// <response code="401">If the user is not authorized to perform this action.</response>
        /// <response code="403">If the user does not have the required role (Admin).</response>
        /// <response code="500">If an internal server error occurs during registration.</response>
        [HttpPost("Employee/sign-up")]
        [SwaggerOperation("Register a new employee user.")]
        [Authorize(Roles = nameof(UserRoles.Admin))]
        [ProducesResponseType(typeof(bool) ,               StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResult) ,   StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResult) , StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ForbidResult) ,       StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiProblemDetails) ,     StatusCodes.Status500InternalServerError)]
        public virtual async Task<bool> SingUpEmployeeAsync([FromBody] SingUpEmployeeRequest request , CancellationToken cancellationToken)
        {
            return await Mediator.Send(request.Adapt<RegisterEmployeeCommand>() , cancellationToken);
        }

    
        /// <summary>
        /// Authenticates an admin user by username and password.
        /// </summary>
        /// <param name="request">The request containing the admin user's login credentials.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The response containing the authentication result and user details.</returns>
        /// <response code="200">Returns the authentication result and user details.</response>
        /// <response code="400">If the request is invalid (e.g., missing required fields or invalid credentials).</response>
        /// <response code="401">If the provided credentials are invalid or the user is not authorized.</response>
        /// <response code="500">If an internal server error occurs during authentication.</response>
        [HttpPost("login")]
        [SwaggerOperation("login by username and password")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResponse) ,      StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResult) ,   StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResult) , StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiProblemDetails) ,     StatusCodes.Status500InternalServerError)]
        public virtual async Task<LoginResponse> LoginAsync([FromBody] LoginRequest request, CancellationToken cancellationToken) =>
            (await Mediator.Send(request.Adapt<LoginCommand>() , cancellationToken));


        /// <summary>
        /// Refreshes the access token and provides a new refresh token.
        /// </summary>
        /// <param name="request">The request containing the refresh token.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The response containing the new access token and refresh token.</returns>
        /// <response code="200">Returns the new access token and refresh token.</response>
        /// <response code="400">If the request is invalid (e.g., missing or invalid refresh token).</response>
        /// <response code="401">If the refresh token is expired or invalid.</response>
        /// <response code="500">If an internal server error occurs during token refresh.</response>
        [HttpPost("refreshToken")]
        [SwaggerOperation("Refresh the access token and provide a new refresh token.")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(RefreshTokenResponse) , StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResult) ,     StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResult) ,   StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiProblemDetails) ,       StatusCodes.Status500InternalServerError)]
        public virtual async Task<RefreshTokenResponse> RefreshTokenAsync([FromBody] RefreshTokenRequest request , CancellationToken cancellationToken) =>
            await Mediator.Send(request.Adapt<RefreshTokenCommand>() , cancellationToken);
    }
}
