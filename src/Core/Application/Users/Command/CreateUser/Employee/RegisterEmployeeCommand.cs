using MediatR;
using Shifty.Domain.Enums;
using Shifty.Domain.Users;
using System.Collections.Generic;

namespace Shifty.Application.Users.Command.CreateUser.Employee;

public record RegisterEmployeeCommand(
    string FirstName
    , string LastName
    , string FatherName
    , string NationalCode
    , GenderType Gender
    , bool IsLeader
    , string MobileNumber
    , List<string> RolesList
) : IRequest<bool>;