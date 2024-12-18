using Shifty.Application.Users.Command.CreateUser.Admin;
using Shifty.Domain.Enums;
using Shifty.Domain.Users;
using System;
using System.Collections.Generic;

namespace Shifty.Application.Users.Requests;

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

    public List<string> RolesList { get; set; }
    public Guid? DepartmentId { get; set; }
}