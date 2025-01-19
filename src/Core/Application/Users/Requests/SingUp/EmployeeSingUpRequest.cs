using Shifty.Domain.Users;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace Shifty.Application.Users.Requests.SingUp
{
    public class EmployeeSingUpRequest
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

        public string Address { get; set; }
        public Guid? DepartmentId { get; set; }
    }

    public class EmployeeSingUpRequestExample : IExamplesProvider<EmployeeSingUpRequest>
    {
        public EmployeeSingUpRequest GetExamples()
        {
            return new EmployeeSingUpRequest
            {
                FirstName      = "John" , LastName          = "Doe" , FatherName = "Michael" , NationalCode        = "1234567890"
                , Gender       = GenderType.Male , IsLeader = true , PhoneNumber = "09134041409" , PersonnelNumber = "EMP12345"
                , RolesList    = ["Admin" , "Manager"] , // Example roles
                Address        = "123 Main St, City, Country"
                , DepartmentId = Guid.CreateVersion7() , // Example department ID
            };
        }
    }
}