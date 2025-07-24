namespace Shifty.Application.Companies.Requests.InitialCompany;

public class InitialCompanyRequest
{
    public required string Name { get; set; }
    public required string Domain { get; set; }

    public required string PhoneNumber { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Password { get; set; }
    public required string ConfirmPassword { get; set; }
}