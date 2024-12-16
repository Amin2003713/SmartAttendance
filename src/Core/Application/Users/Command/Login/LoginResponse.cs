using Mapster;
using Shifty.Domain.Entities.Users;
using Shifty.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Shifty.Application.Users.Command.Login
{
    public class LoginResponse
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public UserInfo UserInfo { get; set; }
    }

    public class UserInfo
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public GenderType Gender { get; set; }
        public bool IsTeamLeader { get; set; }
        public string MobileNumber { get; set; }
        public string EmployeeId { get; set; }
        public string ProfilePicture { get; set; }
        public string Address { get; set; }
        public List<string> RolesList { get; set; }

      

        public static UserInfo CreateInstance(User user, List<string> rolesList)
        {
            var userInfo =  user.Adapt<UserInfo>();
userInfo.RolesList = rolesList;
return userInfo;
        }
    }
}
