namespace Shifty.Application.Users.Command.Verify
{
    public class VerifyPhoneNumberResponse
    {
        public bool IsVerified { get; set; }
        public string? Message { get; set; }
    }
}