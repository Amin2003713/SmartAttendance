using Swashbuckle.AspNetCore.Filters;

namespace Shifty.Application.Users.Requests.Login
{
    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
    }

    public class RefreshTokenRequestExample : IExamplesProvider<RefreshTokenRequest>
    {
        public RefreshTokenRequest GetExamples()
        {
            return new RefreshTokenRequest
            {
                RefreshToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..." , // Example refresh token
                AccessToken  = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..." ,  // Example access token
            };
        }
    }
}