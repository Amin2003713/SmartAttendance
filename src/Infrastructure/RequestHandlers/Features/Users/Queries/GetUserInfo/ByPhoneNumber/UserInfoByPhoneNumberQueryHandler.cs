using Shifty.Application.Features.Users.Queries.GetUserInfo.ByPhoneNumber;
using Shifty.Application.Features.Users.Requests.Queries.GetUserInfo.ByPhoneNumber;

namespace Shifty.RequestHandlers.Features.Users.Queries.GetUserInfo.ByPhoneNumber;

/// <summary>
///     Handles the <see cref="UserInfoByPhoneNumberQuery" />, returning user information based on a phone number.
/// </summary>
public class
    UserInfoByPhoneNumberQueryHandler : IRequestHandler<UserInfoByPhoneNumberQuery, UserInfoByPhoneNumberResponse>
{
    /// <summary>
    ///     Processes the <see cref="UserInfoByPhoneNumberQuery" />.
    /// </summary>
    /// <param name="request">The query containing the phone number.</param>
    /// <param name="cancellationToken">A token that may be used to cancel the operation.</param>
    /// <returns>A task that returns <see cref="UserInfoByPhoneNumberResponse" /> when completed.</returns>
    public async Task<UserInfoByPhoneNumberResponse> Handle(
        UserInfoByPhoneNumberQuery request,
        CancellationToken cancellationToken)
    {
        return new UserInfoByPhoneNumberResponse();
    }
}