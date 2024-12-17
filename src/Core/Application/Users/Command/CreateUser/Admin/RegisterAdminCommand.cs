using MediatR;
using Shifty.Domain.Users;

namespace Shifty.Application.Users.Command.CreateUser.Admin;

public class RegisterAdminCommand : IRequest<bool>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string FatherName { get; set; }

    public string NationalCode { get; set; }


    public GenderType Gender { get; set; }

    public string MobileNumber { get; set; }

    public string ProfilePicture { get; set; }

    public string Address { get; set; }

    public CoInfo CompanyInfo { get; set; }

}

public class CoInfo
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string WebSite { get; set; }
    public string PhoneNumber { get; set; }
    public string ApplicationAccessDomain { get; set; }
}



