using MediatR;
using Shifty.Domain.Users;
using System;
using System.Collections.Generic;

namespace Shifty.Application.Users.Command.CreateUser.Employee
{
    public class RegisterEmployeeCommand : IRequest<bool>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public List<string> RolesList { get; set; }
        public string Address { get; set; }
        public Guid? DivisionId { get; set; }
    }
}