using Shifty.Domain.Users;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace Shifty.Application.Users.Requests.SingUp
{
    public class EmployeeSingUpRequestExample : IExamplesProvider<EmployeeSingUpRequest>
    {
        public EmployeeSingUpRequest GetExamples()
        {
            return new EmployeeSingUpRequest
            {
                FirstName = "John" , LastName          = "Doe" , FatherName = "Michael" , NationalCode        = "1234567890" ,
                Gender    = GenderType.Male , IsLeader = true , PhoneNumber = "09134041409" , PersonnelNumber = "EMP12345" ,
                RolesList = ["Admin" , "Manager"] ,                                               // Example roles
                Address   = "123 Main St, City, Country" , DepartmentId = Guid.CreateVersion7() , // Example department ID
            };
        }
    }
}