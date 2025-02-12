using MediatR;
using Shifty.Application.Users.Queries.GetUserInfo.ByPhoneNumber;
using Shifty.Application.Users.Requests.Queries.GetUserInfo.ByPhoneNumber;

namespace Shifty.RequestHandlers.Users.Queries.GetUserInfo.ByPhoneNumber;

/// <summary>
/// Handles the <see cref="UserInfoByPhoneNumberQuery"/>, returning user information based on a phone number.
/// </summary>
public class UserInfoByPhoneNumberQueryHandler : IRequestHandler<UserInfoByPhoneNumberQuery, UserInfoByPhoneNumberResponse>
{
    /// <summary>
    /// Processes the <see cref="UserInfoByPhoneNumberQuery"/>.
    /// </summary>
    /// <param name="request">The query containing the phone number.</param>
    /// <param name="cancellationToken">A token that may be used to cancel the operation.</param>
    /// <returns>A task that returns <see cref="UserInfoByPhoneNumberResponse"/> when completed.</returns>
    public Task<UserInfoByPhoneNumberResponse> Handle(UserInfoByPhoneNumberQuery request, CancellationToken cancellationToken)
    {
        // TODO: Implement logic to retrieve user information from your data store.
        throw new NotImplementedException();
    }
}