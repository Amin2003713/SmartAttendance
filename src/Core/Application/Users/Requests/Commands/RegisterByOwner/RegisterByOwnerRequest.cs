namespace Shifty.Application.Users.Requests.Commands.RegisterByOwner;

public class RegisterByOwnerRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }

    public List<string> Roles { get; set; }
}