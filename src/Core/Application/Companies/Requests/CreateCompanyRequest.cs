using System;

namespace Shifty.Application.Companies.Requests
{
    public class CreateCompanyRequest
    {
        public required string Name { get; set; } // Company name
        public required string Identifier { get; set; } // Tenant ID for the domain
        public required string NationalId { get; set; } // National ID for the company
        public required string RegistrationNumber { get; set; } // Company registration number
        public required string EconomicCode { get; set; } // Economic code (کد اقتصادی)
        public required string Address { get; set; } // Physical address
        public required string PostalCode { get; set; } // Postal code
        public required string PhoneNumber { get; set; } // Contact number
        public required string Email { get; set; } // Company email address
        public required Guid UserId { get; set; } // Company email address
    }


}