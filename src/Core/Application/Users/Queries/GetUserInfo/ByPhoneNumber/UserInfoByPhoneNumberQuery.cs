using MediatR;
using Shifty.Application.Users.Requests.Queries.GetUserInfo.ByPhoneNumber;

namespace Shifty.Application.Users.Queries.GetUserInfo.ByPhoneNumber;

/// <summary>
/// A query object to retrieve user information by phone number.
/// </summary>
public class UserInfoByPhoneNumberQuery : UserInfoByPhoneNumberRequest, IRequest<UserInfoByPhoneNumberResponse>
{
}