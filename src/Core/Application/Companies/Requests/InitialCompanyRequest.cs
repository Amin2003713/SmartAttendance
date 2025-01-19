using Swashbuckle.AspNetCore.Filters;

namespace Shifty.Application.Companies.Requests
{
    public class InitialCompanyRequest
    {
        public required string Name { get; set; }
        public required string Domain { get; set; }
        public string? LandLine { get; set; } = null!;


        public required string PhoneNumber { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }

  
        public class InitialCompanyRequestExample : IExamplesProvider<InitialCompanyRequest>
        {
            public InitialCompanyRequest GetExamples()
            {
                return new InitialCompanyRequest
                {
                    Name   = "Tech Solutions Inc." , // Example company name
                    Domain = "techsolutions"
                    , // Example domain
                    LandLine = "03132222332"
                    , // Example landline number (optional)
                    PhoneNumber = "09134041709"
                    , // Example phone number
                    FirstName = "John"
                    ,                // Example first name
                    LastName = "Doe" // Example last name
                };
            }
        }
    }
