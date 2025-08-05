namespace Shifty.Application.Users.Requests.Queries.GetUserInfo.ByPhoneNumber;

/// <summary>
///     Provides an example for <see cref="UserInfoByPhoneNumberResponse" />.
/// </summary>
public class UserInfoByPhoneNumberResponseExample : IExamplesProvider<UserInfoByPhoneNumberResponse>
{
    /// <inheritdoc />
    public UserInfoByPhoneNumberResponse GetExamples()
    {
        // Return sample data for documentation
        return new UserInfoByPhoneNumberResponse
        {
            // For example:
            // FirstName = "John",
            // LastName = "Doe",
            // DateOfBirth = new DateTime(1990, 1, 1)
        };
    }
}