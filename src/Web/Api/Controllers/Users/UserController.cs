using Shifty.Application.Users.Command.CreateUser.Employee;
using Shifty.Application.Users.Command.ForgotPassword;
using Shifty.Application.Users.Command.Login;
using Shifty.Application.Users.Command.RefreshToken;
using Shifty.Application.Users.Command.Verify;
using Shifty.Application.Users.Queries.SendActivationCode;
using Shifty.Application.Users.Requests.Commands.ForgotPassword;
using Shifty.Application.Users.Requests.Commands.Login;
using Shifty.Application.Users.Requests.Commands.SingUp;
using Shifty.Application.Users.Requests.Commands.Verify;
using Shifty.Domain.Enums;
using Swashbuckle.AspNetCore.Annotations;

namespace Shifty.Api.Controllers.Users
{
    public class UserController : BaseController
    {







        /// <summary>
        /// Initiates a "forgot password" process by getting new code from the user.
        /// </summary>
        /// <param name="request">An object containing the user name and  password.</param>
        /// <param name="cancellationToken">A token for cancelling the request.</param>
        /// <returns>A response indicating whether the email was sent successfully.</returns>
        /// <response code="200">Returns a success message indicating an email was sent (or that the email does not exist).</response>
        /// <response code="400">If the request is invalid (e.g., missing or invalid email address).</response>
        /// <response code="403">If an error occurs with password.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [HttpPut("forgot-password")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResult) ,       StatusCodes.Status403Forbidden)]
        public async Task ForgotPasswordAsync(
            [FromBody] ForgotPasswordRequest request ,
            CancellationToken cancellationToken)
            => await Mediator.Send(request.Adapt<ForgotPasswordCommand>() , cancellationToken);


        /// <summary>
        ///     Authenticates an admin user by username and password.
        /// </summary>
        /// <param name="request">The request containing the admin user's login credentials.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The response containing the authentication result and user details.</returns>
        /// <response code="200">Returns the authentication result and user details.</response>
        /// <response code="400">If the request is invalid (e.g., missing required fields or invalid credentials).</response>
        /// <response code="401">If the provided credentials are invalid or the user is not authorized.</response>
        /// <response code="500">If an internal server error occurs during authentication.</response>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResponse) ,      StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResult) ,   StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResult) , StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiProblemDetails) ,  StatusCodes.Status500InternalServerError)]
        public virtual async Task<LoginResponse> LoginAsync([FromBody] LoginRequest request , CancellationToken cancellationToken)
        {
            return await Mediator.Send(request.Adapt<LoginCommand>() , cancellationToken);
        }


        /// <summary>
        ///     Refreshes the access token and provides a new refresh token.
        /// </summary>
        /// <param name="request">The request containing the refresh token.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The response containing the new access token and refresh token.</returns>
        /// <response code="200">Returns the new access token and refresh token.</response>
        /// <response code="400">If the request is invalid (e.g., missing or invalid refresh token).</response>
        /// <response code="401">If the refresh token is expired or invalid.</response>
        /// <response code="500">If an internal server error occurs during token refresh.</response>
        [HttpPost("refresh-token")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(RefreshTokenResponse) , StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResult) ,     StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResult) ,   StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiProblemDetails) ,    StatusCodes.Status500InternalServerError)]
        public virtual async Task<RefreshTokenResponse> RefreshTokenAsync([FromBody] RefreshTokenRequest request , CancellationToken cancellationToken)
        {
            return await Mediator.Send(request.Adapt<RefreshTokenCommand>() , cancellationToken);
        }

        /// <summary>
        ///     Verifies the code sent to the user's phone number.
        /// </summary>
        /// <param name="request">The request containing the phone number and code.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The response indicating whether the verification was successful.</returns>
        /// <response code="200">Returns the verification result.</response>
        /// <response code="400">If the request is invalid (e.g., missing phone number or code).</response>
        /// <response code="500">If an internal server error occurs during verification.</response>
        [HttpPut("verify")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(VerifyPhoneNumberResponse) , StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResult) ,          StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiProblemDetails) ,         StatusCodes.Status500InternalServerError)]
        public virtual async Task<VerifyPhoneNumberResponse> VerifyPhoneNumberAsync(
            [FromBody] VerifyPhoneNumberRequest request ,
            CancellationToken cancellationToken)
        {
            return await Mediator.Send(request.Adapt<VerifyPhoneNumberCommand>() , cancellationToken);
        }

        /// <summary>
        ///     Sends an activation code to the specified user.
        /// </summary>
        /// <param name="phoneNumber">The request containing the user's phone number.</param>
        /// <returns>The response containing the status of the activation code sending process.</returns>
        /// <response code="200">Returns the status of the activation code sending process.</response>
        /// <response code="400">If the request is invalid (e.g., missing or invalid user ID).</response>
        /// <response code="404">If the user with the specified ID is not found.</response>
        /// <response code="500">If an internal server error occurs while sending the activation code.</response>
        [HttpGet("send-activation-code")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(SendActivationCodeQueryResponse) , StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResult) ,                StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(NotFoundResult) ,                  StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiProblemDetails) ,               StatusCodes.Status500InternalServerError)]
        public virtual async Task<SendActivationCodeQueryResponse> SendActivationCode([FromQuery] string phoneNumber , CancellationToken cancellationToken)
        {
            return await Mediator.Send(SendActivationCodeQuery.Created(phoneNumber) , cancellationToken);
        }


        /// <summary>
        ///     Registers a new employee user.
        /// </summary>
        /// <param name="request">The request containing the employee's details.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>True if the registration was successful; otherwise, false.</returns>
        /// <response code="200">Returns true if the registration was successful.</response>
        /// <response code="400">If the request is invalid (e.g., missing required fields or invalid data).</response>
        /// <response code="401">If the user is not authorized to perform this action.</response>
        /// <response code="403">If the user does not have the required role (Admin).</response>
        /// <response code="500">If an internal server error occurs during registration.</response>
        [HttpPost("employee/sign-up")]
        [Authorize(Roles = nameof(UserRoles.Admin))]
        [ProducesResponseType(typeof(bool) ,               StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResult) ,   StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResult) , StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ForbidResult) ,       StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiProblemDetails) ,  StatusCodes.Status500InternalServerError)]
        public virtual async Task<bool> SingUpEmployeeAsync([FromBody] EmployeeSingUpRequest request , CancellationToken cancellationToken)
        {
            return await Mediator.Send(request.Adapt<RegisterEmployeeCommand>() , cancellationToken);
        }
    }
}