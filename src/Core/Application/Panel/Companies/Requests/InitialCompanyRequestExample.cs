using Swashbuckle.AspNetCore.Filters;

namespace Shifty.Application.Panel.Companies.Requests
{
    public class InitialCompanyRequestExample : IExamplesProvider<InitialCompanyRequest>
    {
        public InitialCompanyRequest GetExamples()
        {
            return new InitialCompanyRequest
            {
                Name        = "Tech Solutions Inc." , // Example company name
                Domain      = "techsolutions" ,       // Example domain
                LandLine    = "03132222332" ,         // Example landline number (optional)
                PhoneNumber = "09134041709" ,         // Example phone number
                FirstName   = "John" ,                // Example first name
                LastName    = "Doe" ,                 // Example last name
            };
        }
    }
}