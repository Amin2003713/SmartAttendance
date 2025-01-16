using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Shifty.Application.Companies.Command.InitialCompany;

public class InitialCompanyCommand : IRequest<IActionResult>
{
    public required string Domain { get; set; }
    public required string OrganizationName { get; set; }
    public string? LandLine { get; set; } = null!;


    public required string PhoneNumber { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}

