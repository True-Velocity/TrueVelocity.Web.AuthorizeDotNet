namespace Bet.Extensions.AuthorizeNet.Models.V1
{
    public class Address
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string CompanyName { get; set; } = string.Empty;

        public string StreetLine { get; set; } = string.Empty;

        public string StateOrProvince { get; set; } = string.Empty;

        public string ZipCode { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;
    }
}
