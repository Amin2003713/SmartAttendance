namespace Shifty.Application.Users.Requests.Commands.UpdatePhoneNumber;

public class UpdatePhoneNumberRequest
{
    public string PhoneNumber { get; set; } = null!;
    public string Code { get; set; } = null!;
}