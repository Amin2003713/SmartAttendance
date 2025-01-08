using Mapster;
using Shifty.Domain.Enums;
using Shifty.Domain.Users;
using System;
using System.Collections.Generic;

namespace Shifty.Application.Users.Command.Login
{
    public class LoginResponse
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public GenderType Gender { get; set; }
        public bool IsTeamLeader { get; set; }
        public string PhoneNumber { get; set; }
        public string? EmployeeId { get; set; }
        public string? ProfilePicture { get; set; }
        public string Address { get; set; }
        public List<string>? RolesList { get; set; } = [];


        public LoginResponse AddToken(string refreshToken , string token , List<string>? rolesList)
        {
            RefreshToken = refreshToken;
            Token        = token;
            RolesList    = rolesList ?? [] ;

            return this;
        }
    }
}
