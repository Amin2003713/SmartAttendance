using SmartAttendance.Common.General.Enums;
using SmartAttendance.Common.General.Enums.Genders;

namespace SmartAttendance.Application.Features.Users.Requests.Commands.RegisterByOwner;

public class RegisterByOwnerRequest
{
    public string? ImageUrl { set; get; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string FatherName { get; set; }

    public string NationalCode { get; set; }

    public bool IsLeader { get; set; }

    public GenderType Gender { get; set; }

    public Guid? DepartmentId { get; set; }

    public string PersonnelNumber { get; set; }

    public UserType Type { get; set; }

    public string PhoneNumber { get; set; }

}