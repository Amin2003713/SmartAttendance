using Swashbuckle.AspNetCore.Filters;

namespace Shifty.Application.Companies.Requests.InitialCompany;

public class InitialCompanyRequestExample : IExamplesProvider<InitialCompanyRequest>
{
    public InitialCompanyRequest GetExamples()
    {
        return new InitialCompanyRequest
        {
            Name = "Tech Solutions Inc.", // Example company name
            Domain = "aaa",               // Example domain
            PhoneNumber = "09134041709",  // Example phone number
            FirstName = "John",           // Example first name
            LastName = "Doe",             // Example last name
            Password = "@AminDrp1",
            ConfirmPassword = "@AminDrp1"
        };
    }
}