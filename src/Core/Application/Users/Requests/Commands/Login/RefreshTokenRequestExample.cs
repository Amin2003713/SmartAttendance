using Swashbuckle.AspNetCore.Filters;

namespace Shifty.Application.Users.Requests.Commands.Login
{
    public class RefreshTokenRequestExample : IExamplesProvider<RefreshTokenRequest>
    {
        public RefreshTokenRequest GetExamples()
        {
            return new RefreshTokenRequest
            {
                RefreshToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..." , // Example refresh token
                AccessToken  = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..." , // Example access token
            };
        }
    }
}