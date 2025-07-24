namespace Shifty.Application.Companies.Commands.InitialCompany;

public class InitialCompanyCommand : IRequest<string>
{
    public required string Domain { get; set; }
    public required string Name { get; set; }


    public required string PhoneNumber { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Password { get; set; }

    public required string ConfirmPassword { get; set; }
}