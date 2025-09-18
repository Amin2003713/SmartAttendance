using SmartAttendance.Common.Common.Requests;
using SmartAttendance.Common.General.Enums;
using SmartAttendance.Common.General.Enums.Genders;

namespace SmartAttendance.Application.Features.Users.Requests.Commands.RegisterByOwner;

public class RegisterByOwnerRequest
{
    public string PersonalNumber { get; set; } = null!;

    public Roles Role { get; set; }
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? FatherName { get; set; }

    public string? NationalCode { get; set; }

    public string? PhoneNumber { get; set; }

    public DateTime? BirthDate { get; set; }

    public string? Address { get; set; }
    public GenderType Gender { get; set; }
    public UploadMediaFileRequest? ProfilePicture { get; set; }

    public bool? IsActive { get; set; }
}