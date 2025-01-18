using Microsoft.AspNetCore.Identity;
using Shifty.Domain.Common.BaseClasses;
using Shifty.Domain.Tenants;
using System;
using System.Collections.Generic;

namespace Shifty.Domain.Users
{
    public class User : IdentityUser<Guid>, IEntity
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


        public bool IsActive { get; set; } = false;

        public Guid CreatedBy { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Guid? ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }

        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime LastLoginDate { get; set; }

        // [ForeignKey(nameof(Department))]
        // public Guid? DepartmentId { get; set; }

        // public virtual Department Department { get; set; }
        //
        // // Relationships
        // public virtual ICollection<UserRole> UserRoles { get; set; }
        // public virtual ICollection<MissionUser> MissionAssignments { get; set; }
        // public virtual ICollection<Record> Records { get; set; }
        // public virtual ICollection<MissionRecord> MissionRecords { get; set; }
        // public virtual ICollection<Leave> LeaveRequests { get; set; }
        // public virtual ICollection<Department> ManagedDepartments { get; set; }
        // public virtual ICollection<MessageUser> MessageRecipients { get; set; }
        // public virtual ICollection<Message> SentMessages { get; set; }
        // public virtual ICollection<Payslip> Payslips { get; set; }



        public void SetUserName() => UserName = PhoneNumber;

        public void SetPasswordHash(string hashPassword) =>
            PasswordHash = hashPassword;
    }
}