namespace Shifty.Application.Companies.Queries.GetCompanyInfo
{
    public class GetCompanyInfoResponse
    {
        public string Id { get; set; }
        public string Identifier { get; set; }
        public string Name { get; set; }
        public string NationalId { get; set; }
        public string RegistrationNumber { get; set; }
        public string EconomicCode { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}