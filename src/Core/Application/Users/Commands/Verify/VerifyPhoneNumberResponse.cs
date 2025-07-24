namespace Shifty.Application.Users.Commands.Verify;

public class VerifyPhoneNumberResponse
{
    public bool IsVerified { get; set; }
    public string? Message { get; set; }
}