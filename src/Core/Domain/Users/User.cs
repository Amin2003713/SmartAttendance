using SmartAttendance.Common.General.BaseClasses;
using SmartAttendance.Common.General.Enums.Genders;
using SmartAttendance.Common.General.Enums.RoleTypes;


namespace SmartAttendance.Domain.Users;

public class User : IdentityUser<Guid>,
    IEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string FatherName { get; set; }

    public string NationalCode { get; set; }

    public GenderType Gender { get; set; }

    public bool IsLeader { get; set; }

    public RoleType roleType { get; set; }

    public string? PersonnelNumber { get; set; }
    public string? Profile { get; set; }
    public string? Address { get; set; }
    public DateTime? LastActionOnServer { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime? BirthDate { get; set; }
    public override Guid Id { get; set; } = Guid.CreateVersion7(DateTimeOffset.Now);

    public void SetPasswordHash(string hashPassword)
    {
        UserName = PhoneNumber;
        NormalizedUserName = PhoneNumber!.ToUpper();
        SecurityStamp = Guid.NewGuid().ToString();
        ConcurrencyStamp = Guid.NewGuid().ToString();
        PasswordHash = hashPassword;
        Email = $"{PhoneNumber}@gmail.com";
        NormalizedEmail = Email.ToUpper();
    }


    public string FullName()
    {
        return FirstName + " " + LastName;
    }

    public void Update(User source)
    {
        FirstName = source.FirstName;
        LastName = source.LastName;
        Profile = source.Profile;
        Address = source.Address;
        BirthDate = source.BirthDate;
        ModifiedAt = DateTime.UtcNow;
    }

#region log_props

    public bool IsActive { get; set; } = true;
    public Guid? CreatedBy { get; set; } = null;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid? ModifiedBy { get; set; } = null;
    public DateTime? ModifiedAt { get; set; }
    public Guid? DeletedBy { get; set; } = null!;
    public DateTime? DeletedAt { get; set; } = null!;
    public IEnumerable<UserPassword>? UserPasswords { get; set; }

#endregion
}