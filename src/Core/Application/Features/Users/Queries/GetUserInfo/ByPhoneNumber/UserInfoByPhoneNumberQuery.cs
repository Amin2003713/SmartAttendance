using SmartAttendance.Application.Features.Users.Requests.Queries.GetUserInfo.ByPhoneNumber;

namespace SmartAttendance.Application.Features.Users.Queries.GetUserInfo.ByPhoneNumber;

/// <summary>
///     A query object to retrieve user information by phone number.
/// </summary>
public class UserInfoByPhoneNumberQuery : UserInfoByPhoneNumberRequest,
    IRequest<UserInfoByPhoneNumberResponse> { }