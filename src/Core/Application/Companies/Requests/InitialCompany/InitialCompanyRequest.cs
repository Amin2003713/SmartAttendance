namespace Shifty.Application.Companies.Requests.InitialCompany;

public class InitialCompanyRequest
{
    public required string Domain { get; set; }

    public required string Name { get; set; }
    public string NationalCode { get; set; }

    public required string PhoneNumber { get; set; }

    public required string FirstName { get; set; }
    public required string FatherName { get; set; }

    public required string LastName { get; set; }
}