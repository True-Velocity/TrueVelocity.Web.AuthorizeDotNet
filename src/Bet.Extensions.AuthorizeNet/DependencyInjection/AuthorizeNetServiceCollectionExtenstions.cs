using System;

using AuthorizeNet.Api.V1.Contracts;

using Bet.Extensions.AuthorizeNet.Api.V1.Clients;
using Bet.Extensions.AuthorizeNet.Options;

using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AuthorizeNetServiceCollectionExtenstions
    {
        public static IServiceCollection AddAuthorizeNet(
            this IServiceCollection services,
            string sectionName = nameof(AuthorizeNetOptions),
            Action<AuthorizeNetOptions>? configureOptions = default)
        {
            services.AddChangeTokenOptions<AuthorizeNetOptions>(sectionName, configureAction: o => configureOptions?.Invoke(o));

            services.AddScoped(sp =>
            {
                var options = sp.GetRequiredService<IOptionsMonitor<AuthorizeNetOptions>>().CurrentValue;

                return new MerchantAuthenticationType
                {
                    Name = options.ClientName,
                    TransactionKey = options.TransactionKey
                };
            });

            services.AddHttpClient(string.Empty, (sp, configure) =>
            {
                var options = sp.GetRequiredService<IOptionsMonitor<AuthorizeNetOptions>>().CurrentValue;
                configure.BaseAddress = options.LetsEncryptUri;
            });

            services.AddScoped<CustomerProfileClient>();

            return services;
        }
    }
}
