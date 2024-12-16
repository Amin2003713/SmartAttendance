using Shifty.Domain.Entities.Users;
using System;
using System.Collections.Generic;

namespace Shifty.Api.Controllers.v1.Users.Requests;

public class SingUpEmployeeRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FatherName { get; set; }
    public string NationalCode { get; set; }
    public GenderType Gender { get; set; }
    public bool IsLeader { get; set; }
    public string MobileNumber { get; set; }
    public string PersonnelNumber { get; set; }

    public Guid? DepartmentId { get; set; }
}

public class SingUpAdminRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string FatherName { get; set; }

    public string NationalCode { get; set; }


    public GenderType Gender { get; set; }


    public string MobileNumber { get; set; }



    public string ProfilePicture { get; set; }


    public string Address { get; set; }

}
