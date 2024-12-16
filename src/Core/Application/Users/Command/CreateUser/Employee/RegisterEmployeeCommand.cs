using Shifty.Domain.Entities.Users;
using MediatR;
using Shifty.Domain.Enums;
using System.Collections.Generic;

public record RegisterEmployeeCommand(
    string FirstName
    , string LastName
    , string FatherName
    , string NationalCode
    , GenderType Gender
    , bool IsLeader
    , string Mobile
    , List<UserRoles> RolesList
) : IRequest<bool>;