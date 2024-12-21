using Shifty.Domain.Users;
using System;
using System.Collections.Generic;

namespace Shifty.Application.Users.Requests.SingUp;

public class SingUpEmployeeRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FatherName { get; set; }
    public string NationalCode { get; set; }
    public GenderType Gender { get; set; }
    public bool IsLeader { get; set; }
    public string PhoneNumber { get; set; }
    public string PersonnelNumber { get; set; }

    public List<string> RolesList { get; set; }
    public Guid? DepartmentId { get; set; }
}