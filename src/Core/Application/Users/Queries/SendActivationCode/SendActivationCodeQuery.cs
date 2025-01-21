using MediatR;

namespace Shifty.Application.Users.Queries.SendActivationCode
{
    public class SendActivationCodeQuery : IRequest<SendActivationCodeQueryResponse>
    {
        public string PhoneNumber { get; set; }

        public static SendActivationCodeQuery Created(string phoneNumber)
        {
            return new SendActivationCodeQuery
            {
                PhoneNumber = phoneNumber ,
            };
        }
    }
}