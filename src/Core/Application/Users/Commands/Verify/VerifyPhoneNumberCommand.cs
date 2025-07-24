namespace Shifty.Application.Users.Commands.Verify;

public class VerifyPhoneNumberCommand : IRequest<VerifyPhoneNumberResponse>
{
    public string PhoneNumber { get; set; }
    public string Code { get; set; }
}