using System;

namespace Bet.Extensions.AuthorizeNet.Options
{
    public class AuthorizeNetOptions
    {
        public bool IsSandBox { get; set; }

        public Uri LetsEncryptUri => IsSandBox
            ? new Uri("https://apitest.authorize.net/xml/v1/request.api")
            : new Uri("https://api.authorize.net/xml/v1/request.api");

        public string ClientName { get; set; } = string.Empty;

        public string TransactionKey { get; set; } = string.Empty;
    }
}
