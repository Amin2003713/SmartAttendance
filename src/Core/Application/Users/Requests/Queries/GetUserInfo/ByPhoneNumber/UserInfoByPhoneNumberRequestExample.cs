namespace Shifty.Application.Users.Requests.Queries.GetUserInfo.ByPhoneNumber;

/// <summary>
///     Provides an example for <see cref="UserInfoByPhoneNumberRequest" />.
/// </summary>
public class UserInfoByPhoneNumberRequestExample : IExamplesProvider<UserInfoByPhoneNumberRequest>
{
    /// <inheritdoc />
    public UserInfoByPhoneNumberRequest GetExamples()
    {
        return new UserInfoByPhoneNumberRequest
        {
            PhoneNumber = "09134041709"
        };
    }
}