using Microsoft.AspNetCore.Authentication;
using Shifty.Application.Features.Users.Commands.AddRole;
using Shifty.Application.Features.Users.Commands.ForgotPassword;
using Shifty.Application.Features.Users.Commands.Login;
using Shifty.Application.Features.Users.Commands.Logout;
using Shifty.Application.Features.Users.Commands.RefreshToken;
using Shifty.Application.Features.Users.Commands.RegisterByOwner;
using Shifty.Application.Features.Users.Commands.SendActivationCode;
using Shifty.Application.Features.Users.Commands.UpdatePhoneNumber;
using Shifty.Application.Features.Users.Commands.UpdateUser;
using Shifty.Application.Features.Users.Commands.Verify;
using Shifty.Application.Features.Users.Queries.GetAllUsers;
using Shifty.Application.Features.Users.Queries.GetUserInfo.LoggedIn;
using Shifty.Application.Features.Users.Requests.Commands.AddRole;
using Shifty.Application.Features.Users.Requests.Commands.ForgotPassword;
using Shifty.Application.Features.Users.Requests.Commands.Login;
using Shifty.Application.Features.Users.Requests.Commands.RegisterByOwner;
using Shifty.Application.Features.Users.Requests.Commands.SendActivationCode;
using Shifty.Application.Features.Users.Requests.Commands.UpdatePhoneNumber;
using Shifty.Application.Features.Users.Requests.Commands.UpdateUser;
using Shifty.Application.Features.Users.Requests.Commands.Verify;
using Shifty.Application.Features.Users.Requests.Queries.GetUserRoles;
using Shifty.Common.Common.Responses.Users.Queries.Base;
using Shifty.Common.General.Enums;

namespace Shifty.Api.Controllers.Users;

public class UserController : ShiftyBaseController
{
    /// <summary>
    ///     Initiates a "forgot password" process by getting new code from the user.
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
    [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status403Forbidden)]
    public async Task ForgotPasswordAsync(
        [FromBody] ForgotPasswordRequest request,
        CancellationToken cancellationToken)
    {
        await Mediator.Send(request.Adapt<ForgotPasswordCommand>(), cancellationToken);
    }


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
    [ProducesResponseType(typeof(LoginResponse),      StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResult),   StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiProblemDetails),  StatusCodes.Status500InternalServerError)]
    public virtual async Task<LoginResponse> LoginAsync(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        return await Mediator.Send(request.Adapt<LoginCommand>(), cancellationToken);
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
    [ProducesResponseType(typeof(RefreshTokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResult),     StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(UnauthorizedResult),   StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiProblemDetails),    StatusCodes.Status500InternalServerError)]
    public virtual async Task<RefreshTokenResponse> RefreshTokenAsync(
        [FromBody] RefreshTokenRequest request,
        CancellationToken cancellationToken)
    {
        return await Mediator.Send(request.Adapt<RefreshTokenCommand>(), cancellationToken);
    }

    /// <summary>
    ///     Retrieves all users from the system.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A collection of users.</returns>
    /// <response code="200">Users retrieved successfully.</response>
    /// <response code="401">Unauthorized access.</response>
    /// <response code="403">Access to this resource is forbidden.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpGet("List-Users")]
    [ProducesResponseType(typeof(List<GetUserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails),     StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiProblemDetails),     StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiProblemDetails),     StatusCodes.Status500InternalServerError)]
    public async Task<List<GetUserResponse>> GetAllUsers(CancellationToken cancellationToken)
    {
        // Dispatch the query to its handler using the MediatR pattern
        return await Mediator.Send(new GetAllUsersQuery(), cancellationToken);
    }


    /// <summary>
    ///     Verifies the code sent to the user's phone number.
    /// </summary>
    /// <param name="request">The request containing the phone number(UserName) and code.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response indicating whether the verification was successful.</returns>
    /// <response code="200">Returns the verification result.</response>
    /// <response code="400">If the request is invalid (e.g., missing phone number or code).</response>
    /// <response code="500">If an internal server error occurs during verification.</response>
    [HttpPut("verify")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(VerifyPhoneNumberResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResult),          StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiProblemDetails),         StatusCodes.Status500InternalServerError)]
    public virtual async Task<VerifyPhoneNumberResponse> VerifyPhoneNumberAsync(
        [FromBody] VerifyPhoneNumberRequest request,
        CancellationToken cancellationToken)
    {
        return await Mediator.Send(request.Adapt<VerifyPhoneNumberCommand>(), cancellationToken);
    }

    /// <summary>
    ///     Sends an activation code to the specified user.
    /// </summary>
    /// <param name="request">The request containing the user's phone number.</param>
    /// <param name="cancellationToken">fill by auto</param>
    /// <returns>The response containing the status of the activation code sending process.</returns>
    /// <response code="200">Returns the status of the activation code sending process.</response>
    /// <response code="400">If the request is invalid (e.g., missing or invalid user ID).</response>
    /// <response code="404">If the user with the specified ID is not found.</response>
    /// <response code="500">If an internal server error occurs while sending the activation code.</response>
    [HttpPost("send-activation-code")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(SendActivationCodeCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResult),                  StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(NotFoundResult),                    StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiProblemDetails),                 StatusCodes.Status500InternalServerError)]
    public virtual async Task<SendActivationCodeCommandResponse> SendActivationCode(
        [FromBody] SendActivationCodeRequest request,
        CancellationToken cancellationToken)
    {
        return await Mediator.Send(request.Adapt<SendActivationCodeCommand>(), cancellationToken);
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
    [HttpPost("Register-employee")]
    [ProducesResponseType(typeof(bool),               StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResult),   StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ForbidResult),       StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiProblemDetails),  StatusCodes.Status500InternalServerError)]
    public virtual async Task RegisterByOwner(
        [FromBody] RegisterByOwnerRequest request,
        CancellationToken cancellationToken)
    {
        await Mediator.Send(request.Adapt<RegisterByOwnerCommand>(), cancellationToken);
    }

    /// <summary>
    ///     Updates the phone number of a user.
    /// </summary>
    /// <param name="request">The request containing new phone number and verification code.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <response code="204">Phone number updated successfully.</response>
    /// <response code="400">Invalid phone number or code.</response>
    /// <response code="401">Unauthorized access.</response>
    /// <response code="403">Access forbidden.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPut("Update-PhoneNumber")]
    [SwaggerOperation(Summary = "Update Phone Number", Description = "Updates user's phone number using verification.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public virtual async Task UpdatePhoneNumber(
        [FromBody] UpdatePhoneNumberRequest request,
        CancellationToken cancellationToken)
    {
        await Mediator.Send(request.Adapt<UpdatePhoneNumberCommand>(), cancellationToken);
    }


    /// <summary>
    ///     Updates the phone number of a user.
    /// </summary>
    /// <param name="request">The request containing new phone number and verification code.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <response code="204">Phone number updated successfully.</response>
    /// <response code="400">Invalid phone number or code.</response>
    /// <response code="401">Unauthorized access.</response>
    /// <response code="403">Access forbidden.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPut("Update-Profile")]
    [SwaggerOperation(Summary = "Update User", Description = "Updates user.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public virtual async Task UpdateUser([FromForm] UpdateUserRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request.Adapt<UpdateUserCommand>().AddFiles(request.ImageFile!),
            cancellationToken);
    }

    /// <summary>
    ///     Retrieves the logged-in user's profile information.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <response code="200">Returns user profile info.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="403">User is not authorized to access this data.</response>
    /// <response code="500">Internal server error.</response>
    [SwaggerOperation(Summary = "Get User Info",
        Description = "Gets profile information of the currently logged-in user.")]
    [HttpGet("User-Info")]
    [Authorize]
    [ProducesResponseType(typeof(GetUserResponse),    StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestResult),   StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ForbidResult),       StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiProblemDetails),  StatusCodes.Status500InternalServerError)]
    public async Task<GetUserResponse> UserInfo(CancellationToken cancellationToken)
    {
        return await Mediator.Send(new LoggedInUserInfoQuery(), cancellationToken);
    }


    /// <summary>
    ///     Adds a new role to the system.
    /// </summary>
    /// <param name="request">The request containing role name and related permissions.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="204">RoleTypes added successfully.</response>
    /// <response code="400">Invalid role data provided.</response>
    /// <response code="401">Unauthorized access.</response>
    /// <response code="403">Access to this resource is forbidden.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpPut("Update-employee")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task UpdateEmployee([FromBody] UpdateEmployeeRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request.Adapt<UpdateEmployeeCommand>(), cancellationToken);
    }

    /// <summary>
    ///     Retrieves a list of roles available in the system.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="200">Returns the list of roles.</response>
    /// <response code="400">Invalid request parameters provided.</response>
    /// <response code="401">Unauthorized access.</response>
    /// <response code="403">Access to this resource is forbidden.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpGet("Get-RoleTypes")]
    [ProducesResponseType(typeof(IDictionary<string, List<KeyValuePair<Roles, string>>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails),                                      StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiProblemDetails),                                      StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiProblemDetails),                                      StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiProblemDetails),                                      StatusCodes.Status500InternalServerError)]
    public async Task<IDictionary<string, List<KeyValuePair<Roles, string>>>> GetRoles(CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetUserRolesQuery(), cancellationToken);
    }


    /// <summary>
    ///     Logs out the currently authenticated user by revoking their authentication token or session.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="200">Logout was successful.</response>
    /// <response code="401">User is not authenticated.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpPost("Logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task Logout(CancellationToken cancellationToken)
    {
        await HttpContext.SignOutAsync();
        // Optionally: Clear user claims, log activity, etc.
        await Mediator.Send(new LogoutCommand(), cancellationToken);
    }
}