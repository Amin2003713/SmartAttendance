using SmartAttendance.Common.Common.Responses.GetLogPropertyInfo.OperatorLogs;
using SmartAttendance.Common.General.Enums.Genders;

namespace SmartAttendance.Common.Common.Responses.Users.Queries.Base;

public class GetUserResponse
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string FatherName { get; set; }

    

    public GenderType Gender { get; set; }

    public string? PersonalNumber { get; set; } = null!;
    public string? Email { get; set; } = null!;
    public LogPropertyInfoResponse? CreatedBy { get; set; } = null!;


    public string? ProfilePicture { get; set; }
    public string? ProfilePictureCompress { get; set; }
    public string? Address { get; set; }
    public DateTime? LastActionOnServer { get; set; }
    public DateTime? BirthDate { get; set; }
    public DateTime? CreatedAt { get; set; }
    public Guid Id { get; set; }

    public string Role { get; set; }

    public bool IsActive           { get; set; }

    public string? FullName           { get; set; }

    public string? UniversityDomain   { get; set; }
    public string? UniversityName     { get; set; }
}