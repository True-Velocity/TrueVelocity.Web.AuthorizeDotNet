using AuthorizeNet.Api.V1.Contracts;
using AuthorizeNet.Worker.Models;

using Bogus;

namespace AuthorizeNet.Worker;

public class SampleData
{
    public string[] CardNumbers => new[]
    {
                "370000000000002",          // American Express
                "6011000000000012",         // Discover
                "4007000000027",            // Visa
                "4012888818888",            // Visa
                "4111111111111111",         // Visa
                "5424000000000015",         // Mastercard
                "2223000010309703",         // Mastercard
                "2223000010309711" // Mastercard
    };

    public string[] CardCodes => new[]
    {
                "900",      // M    Matched.
                "901",      // N    Does not match.
                "902",      // S    Should be on the card, but is not indicated.
                "903",      // U    The issuer is not certified for CVV processing or has not provided an encryption key.
                "904",      // P    Is not processed.
    };

    public string[] ZipCodes => new[]
    {
                "46282",    // 2    This transaction has been declined.     General bank decline.

                            // Visa Response    Mastercard Response     Discover Response   American Express Response  JCB Response
                "46201",    // A    A   A   A   A   A
                "46203",    // E    E   E   Y   E   E
                "46204",    // G    E   E   N/A     E   E
                "46205",    // N    N   N   N   N   N
                "46207",    // R    R   R   R   R   R
                "46208",    // S    S   S   S   S   S
                "46209",    // U    U   U   U   U   U
                "46211",    // E    W   W   N/A     W   W
                "46214",    // N/A  X   X   N/A     X   X
                "46217",    // Z    Z   Z   Y   Z   Z
    };

    public List<CustomerProfile> GetCustomerProfiles()
    {
        return new Faker<CustomerProfile>()

                    .RuleFor(u => u.ReferenceId, f => f.Random.Number(1, 100).ToString())
                    .RuleFor(u => u.CustomerId, f => $"customer-{f.UniqueIndex}")

                    .RuleFor(u => u.CardNumber, f => f.PickRandom(CardNumbers))
                    .RuleFor(u => u.CardCode, f => f.PickRandom(CardCodes))
                    .RuleFor(u => u.ExpirationDate, f => $"{DateTime.Now.Year}-{DateTime.Now.AddMonths(f.Random.Number(1, 4)).Month.ToString("00")}")

                    .RuleFor(u => u.Company, f => f.Company.CompanyName())

                    .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                    .RuleFor(u => u.LastName, f => f.Name.LastName())
                    .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
                    .RuleFor(u => u.CustomerType, f => f.PickRandom<CustomerTypeEnum>())
                    .RuleFor(u => u.CustomerProfileType, f => f.PickRandom<CustomerProfileTypeEnum>())

                    .RuleFor(u => u.StreetLine, (f, u) => f.Address.StreetAddress())
                    .RuleFor(u => u.City, (f, u) => f.Address.City())

                    // .RuleFor(u => u.ZipCode, (f, u) => f.Address.ZipCode())
                    .RuleFor(u => u.ZipCode, f => f.PickRandom(ZipCodes))
                    .RuleFor(u => u.StateOrProvice, (f, u) => f.Address.State())
                    .RuleFor(u => u.Country, (f, u) => f.Address.CountryCode())

                    .Generate(50);
    }
}
