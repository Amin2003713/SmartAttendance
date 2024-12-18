using Shifty.Domain.Users;

namespace Shifty.Application.Users.Requests
{
    public class SingUpAdminRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FatherName { get; set; }

        public string NationalCode { get; set; }


        public GenderType Gender { get; set; }


        public string MobileNumber { get; set; }



        public string ProfilePicture { get; set; }


        public string Address { get; set; }

    }
}