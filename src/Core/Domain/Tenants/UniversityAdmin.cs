namespace SmartAttendance.Domain.Tenants;

public class UniversityAdmin
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsLeader { get; set; }

    public string FatherName { get; set; }
    public string NationalCode { get; set; }
    public List<UniversityTenantInfo> Tenants { get; set; } = [];
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

    public string? FullName()
    {
        return string.Concat(FirstName + " ", LastName);
    }
}