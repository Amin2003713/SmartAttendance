using Microsoft.Extensions.Localization;
using Shifty.Resources.Common;

namespace Shifty.Resources.Messages
{
    public class ValidationMessages(IStringLocalizer<ValidationMessages> localizer) : BaseLocalizer<ValidationMessages>(localizer)   
    {
        //Domain
        public string Domain_Required() => Localize(nameof(Domain_Required));
        public string Domain_MaxLength()  => Localize(nameof(Domain_MaxLength));
        public string Domain_InvalidCharacters() => Localize(nameof(Domain_InvalidCharacters));

        // First name
        public  string FirstName_Required() => Localize(nameof(FirstName_Required));
        public  string FirstName_Length() => Localize(nameof(FirstName_Length));

        // Last name
        public  string LastName_Required() => Localize(nameof(LastName_Required));
        public  string LastName_Length() => Localize(nameof(LastName_Length));


        // Organization name
        public  string OrganizationName_Required() => Localize(nameof(OrganizationName_Required));

        // Phone number
        public  string PhoneNumber_Required() => Localize(nameof(PhoneNumber_Required));
        public  string PhoneNumber_InvalidFormat() => Localize(nameof(PhoneNumber_InvalidFormat));


        // Login Validations
        public string Username_Required() => Localize(nameof(Username_Required));
        public string Password_Required() => Localize(nameof(Password_Required));
        public string Password_MinLength() => Localize(nameof(Password_MinLength));


        public string Property_RefreshToken() => Localize(nameof(Property_RefreshToken));
        public string Property_AccessToken() => Localize(nameof(Property_AccessToken));

        // Validation Messages
        public string Validation_TokenInvalid() => Localize(nameof(Validation_TokenInvalid));


        public string FatherName_Required() => Localize(nameof(FatherName_Required));
        public string FatherName_Length() => Localize(nameof(FatherName_Length));

        // National Code
        public string NationalCode_Required() => Localize(nameof(NationalCode_Required));
        public string NationalCode_Length() => Localize(nameof(NationalCode_Length));
        public string NationalCode_Numeric() => Localize(nameof(NationalCode_Numeric));

        // Gender
        public string Gender_Required() => Localize(nameof(Gender_Required));

        // Leader Status
        public string IsLeader_Required() => Localize(nameof(IsLeader_Required));

        // Personnel Number
        public string PersonnelNumber_Required() => Localize(nameof(PersonnelNumber_Required));

        // Department
        public string DepartmentId_Required() => Localize(nameof(DepartmentId_Required));


        // Code
        public string Code_Required() => Localize(nameof(Code_Required));
        public string Code_Length() => Localize(nameof(Code_Length));
    }
}

