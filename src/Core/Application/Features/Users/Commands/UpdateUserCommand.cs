using SmartAttendance.Application.Features.Users.Requests;
using SmartAttendance.Application.Features.Users.Responses;

namespace SmartAttendance.Application.Features.Users.Commands;

// Update
public sealed record UpdateUserCommand(
    Guid UserId,
    UpdateUserRequest Request
) : IRequest<UserProfileDto>;

// Delete

// Forgot Password

// Reset Password