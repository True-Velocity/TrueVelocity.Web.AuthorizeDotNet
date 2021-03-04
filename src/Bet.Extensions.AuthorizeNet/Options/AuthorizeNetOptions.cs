using System;

using AuthorizeNet.Api.V1.Contracts;

namespace Bet.Extensions.AuthorizeNet.Options
{
    public class AuthorizeNetOptions
    {
        public bool IsSandBox { get; set; }

        /// <summary>
        /// Returns Api Uri based on IsSandBox flag.
        /// </summary>
        public Uri BaseUri => IsSandBox
            ? new Uri("https://apitest.authorize.net/xml/v1/request.api")
            : new Uri("https://api.authorize.net/xml/v1/request.api");

        /// <summary>
        /// Authorize.Net client name.
        /// </summary>
        public string ClientName { get; set; } = string.Empty;

        /// <summary>
        /// Authorize.Net transaction key.
        /// </summary>
        public string TransactionKey { get; set; } = string.Empty;

        /// <summary>
        /// The IP address of the service if missing used automatic detection.
        /// </summary>
        public string IpAddress { get; set; } = string.Empty;

        public ValidationModeEnum ValidationMode { get; set; }
    }
}
