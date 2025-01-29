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
                FirstName  = "John" , LastName          = "Doe"  
             , PhoneNumber = "09134041409" , 
                RolesList  = ["Admin" , "Manager"] ,                                             // Example roles
                Address    = "123 Main St, City, Country" , DivisionId = Guid.CreateVersion7() , // Example department ID
            };
        }
    }
}