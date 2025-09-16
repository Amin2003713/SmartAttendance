using SmartAttendance.Domain.Common;
using SmartAttendance.Domain.Events;
using SmartAttendance.Domain.ValueObjects;

namespace SmartAttendance.Domain.UserAggregate;

// نقش دامنه

// کاربر دامنه
public sealed class UserAggregate : AggregateRoot<UserId>
{
    private readonly HashSet<RoleId> _roleIds = new();

    public UserAggregate(UserId id, string firstName, string lastName, EmailAddress email, PhoneNumber phone, NationalCode nationalCode)
        : base(id)
    {
        FirstName = Normalize(firstName, "نام");
        LastName = Normalize(lastName,   "نام خانوادگی");
        Email = email ?? throw new DomainValidationException("ایمیل الزامی است.");
        Phone = phone ?? throw new DomainValidationException("تلفن الزامی است.");
        NationalCode = nationalCode ?? throw new DomainValidationException("کد ملی الزامی است.");
    }

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public EmailAddress Email { get; private set; }
    public PhoneNumber Phone { get; private set; }
    public NationalCode NationalCode { get; private set; }

    public int FailedLoginCount { get; private set; }
    public bool IsLocked { get; private set; }
    public DateTime? LockedAtUtc { get; private set; }

    public IReadOnlyCollection<RoleId> Roles => _roleIds;

    private static string Normalize(string value, string field)
    {
        value = value?.Trim() ?? throw new DomainValidationException($"{field} الزامی است.");
        if (value.Length is < 2 or > 100) throw new DomainValidationException($"{field} نامعتبر است.");

        return value;
    }

    public void AssignRole(RoleId roleId)
    {
        if (_roleIds.Add(roleId))
        {
            RaiseDomainEvent(new RoleAssignedEvent(Id, roleId));
        }
    }

    public void RemoveRole(RoleId roleId)
    {
        if (_roleIds.Remove(roleId))
        {
            RaiseDomainEvent(new RoleRemovedEvent(Id, roleId));
        }
    }

    public void RegisterFailedLogin(int thresholdToLock = 5)
    {
        FailedLoginCount++;
        RaiseDomainEvent(new FailedLoginRegisteredEvent(Id, FailedLoginCount));

        if (!IsLocked && FailedLoginCount >= thresholdToLock)
        {
            IsLocked = true;
            LockedAtUtc = DateTime.UtcNow;
            RaiseDomainEvent(new UserLockedEvent(Id));
        }
    }

    public void ResetFailedLogins()
    {
        FailedLoginCount = 0;
    }

    public void Unlock()
    {
        if (!IsLocked) return;

        IsLocked = false;
        LockedAtUtc = null;
        FailedLoginCount = 0;
        RaiseDomainEvent(new UserUnlockedEvent(Id));
    }

    public void UpdateContact(EmailAddress email, PhoneNumber phone)
    {
        Email = email ?? throw new DomainValidationException("ایمیل الزامی است.");
        Phone = phone ?? throw new DomainValidationException("تلفن الزامی است.");
    }

    public bool HasRole(string roleName)
    {
        return roleName != null && _roleIds.Any();
    }
}