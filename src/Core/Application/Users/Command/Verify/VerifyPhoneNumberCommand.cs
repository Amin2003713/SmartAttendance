using MediatR;

namespace Shifty.Application.Users.Command.Verify
{
    public  class VerifyPhoneNumberCommand : IRequest<VerifyPhoneNumberResponse>
    {
        public string PhoneNumber { get; set; }
        public string Code { get; set; }
    }
}