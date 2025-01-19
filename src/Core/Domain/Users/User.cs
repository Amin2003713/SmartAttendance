using Microsoft.AspNetCore.Identity;
using Shifty.Domain.Common.BaseClasses;
using System;

namespace Shifty.Domain.Users
{
    public class User : IdentityUser<Guid> , IEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? FatherName { get; set; } = null!;
        public string? NationalCode { get; set; } = null!;


        public GenderType Gender { get; set; } = GenderType.UnDefine;


        public bool IsTeamLeader { get; set; } = false;


        public string? EmployeeId { get; set; } = null!;

        public string? ProfilePicture { get; set; } = null!;

        public string? NotificationToken { get; set; } = null!;

        public string? Address { get; set; } = null!;

        public string? HardwareId { get; set; } = null!;

        public DateTime LastLoginDate { get; set; }
        public override Guid Id { get; set; } = Guid.CreateVersion7(DateTimeOffset.Now);

        public bool IsActive { get; set; } = true;
        public Guid? CreatedBy { get; set; } = null;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Guid? ModifiedBy { get; set; } = null;
        public DateTime? ModifiedAt { get; set; } = null!;
        public Guid? DeletedBy { get; set; } = null!;
        public DateTime? DeletedAt { get; set; } = null!;


        public void SetUserName()
        {
            UserName = PhoneNumber;
        }

        public void SetPasswordHash(string hashPassword)
        {
            UserName           = PhoneNumber;
            NormalizedUserName = PhoneNumber!.ToUpper();
            SecurityStamp      = Guid.NewGuid().ToString();
            ConcurrencyStamp   = Guid.NewGuid().ToString();
            PasswordHash       = hashPassword;
        }
    }
}