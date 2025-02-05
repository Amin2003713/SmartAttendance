using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Shifty.Domain.Common.BaseClasses;
using Shifty.Domain.Features.Divisions;

namespace Shifty.Domain.Features.Users
{
    public class User : IdentityUser<Guid> , IEntity
    {
        public override Guid Id { get; set; } = Guid.CreateVersion7(DateTimeOffset.Now);
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Profile { get; set; } = null!;
        public string? Address { get; set; } = null!;
        public string? HardwareId { get; set; } = null!;
        public DateTime? LastLoginDate { get; set; } = null!;


        #region log_props

        public bool IsActive { get; set; } = true;
        public Guid? CreatedBy { get; set; } = null;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Guid? ModifiedBy { get; set; } = null;
        public DateTime? ModifiedAt { get; set; } = null!;
        public Guid? DeletedBy { get; set; } = null!;
        public DateTime? DeletedAt { get; set; } = null!;
        

        #endregion


        #region Relations

        public Guid? DivisionId { get; set; }
        public Division? Division { get; set; }

        public List<DivisionAssignee> AssignedDivisions { get; set; }
        public List<Division> Divisions { get; set; }
        #endregion


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