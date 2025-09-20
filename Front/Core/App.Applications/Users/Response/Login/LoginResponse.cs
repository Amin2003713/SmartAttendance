#region

    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using App.Domain.Users;

    namespace App.Applications.Users.Response.Login;

#endregion

    public class LoginResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }


        public UserInfo CreateUser()
        {
            var handler  = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(Token);

            // Helper to get claim by multiple types
            string Get(params string[] types)
                => jwtToken.Claims.FirstOrDefault(c => types.Contains(c.Type))?.Value ?? string.Empty;

            var id        = Get(JwtRegisteredClaimNames.Sub, ClaimTypes.NameIdentifier, "id");
            var userName  = Get(ClaimTypes.Name,             "username");
            var firstName = Get("first_name",                "given_name",  "firstName");
            var lastName  = Get("last_name",                 "family_name", "lastName");
            var profile   = Get("profile");
            var address   = Get("address");
            var email     = Get(JwtRegisteredClaimNames.Email, "email");
            var phone     = Get(ClaimTypes.MobilePhone,        "phoneNumber");

            var roles = jwtToken.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value.ToLower())
                .ToList();

            return new UserInfo
            {
                Id            = id,
                UserName      = userName,
                FirstName     = firstName,
                LastName      = lastName,
                Profile       = string.IsNullOrWhiteSpace(profile) ? null : profile,
                Address       = (string.IsNullOrWhiteSpace(address) ? null : address)!,
                Email         = string.IsNullOrWhiteSpace(email) ? null : email,
                PhoneNumber   = string.IsNullOrWhiteSpace(phone) ? null : phone,
                Roles         = roles,
                LastLoginDate = string.Empty, // can populate later if needed
                Token         = Token
            };
        }
    }