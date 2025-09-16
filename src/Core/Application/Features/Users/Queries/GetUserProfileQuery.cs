using SmartAttendance.Application.Features.Users.Responses;

namespace SmartAttendance.Application.Features.Users.Queries;

// Query: دریافت پروفایل کاربر
public sealed record GetUserProfileQuery(
    Guid UserId
) : IRequest<UserProfileDto>;

// Handler: دریافت پروفایل کاربر