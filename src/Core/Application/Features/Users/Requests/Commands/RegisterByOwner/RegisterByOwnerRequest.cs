using Shifty.Common.General.Enums.Genders;
using Shifty.Common.General.Enums.RoleTypes;

namespace Shifty.Application.Features.Users.Requests.Commands.RegisterByOwner;

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

    public RoleType roleType { get; set; }
   
    public string PhoneNumber { get; set; }

    public List<string> Roles { get; set; }
}