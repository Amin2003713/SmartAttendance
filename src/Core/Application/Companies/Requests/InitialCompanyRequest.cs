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
}