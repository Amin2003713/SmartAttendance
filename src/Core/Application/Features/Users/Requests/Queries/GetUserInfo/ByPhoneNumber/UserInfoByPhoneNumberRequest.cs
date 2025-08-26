namespace Shifty.Application.Features.Users.Requests.Queries.GetUserInfo.ByPhoneNumber;

/// <summary>
///     A request containing the phone number used to retrieve user information.
/// </summary>
public class UserInfoByPhoneNumberRequest
{
    /// <summary>
    ///     The phone number for which to retrieve the user info.
    /// </summary>
    public string PhoneNumber { get; set; }
}