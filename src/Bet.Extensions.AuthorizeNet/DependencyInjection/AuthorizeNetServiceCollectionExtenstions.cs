using AuthorizeNet.Api.V1.Contracts;

using Bet.Extensions.AuthorizeNet;
using Bet.Extensions.AuthorizeNet.Api.V1.Clients;
using Bet.Extensions.AuthorizeNet.Api.V1.Clients.Impl;
using Bet.Extensions.AuthorizeNet.Options;

using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

using Polly;

namespace Microsoft.Extensions.DependencyInjection;

public static class AuthorizeNetServiceCollectionExtenstions
{
    /// <summary>
    /// Adds Authorize.Net Clients for DI.
    /// </summary>
    /// <param name="services">DI services.</param>
    /// <param name="name">The name of the HttpClient to be used for AuthorizeNet calls.</param>
    /// <param name="sectionName">The name of the configurations section.</param>
    /// <param name="configureOptions">The configuration for <see cref="AuthorizeNetOptions"/>.</param>
    /// <param name="retryPolicy">The Polly HttpClient Retry Policy.</param>
    /// <returns></returns>
    public static IServiceCollection AddAuthorizeNet(
        this IServiceCollection services,
        string? name = null,
        string sectionName = nameof(AuthorizeNetOptions),
        Action<AuthorizeNetOptions>? configureOptions = default,
        Func<IServiceProvider, HttpRequestMessage, IAsyncPolicy<HttpResponseMessage>>? retryPolicy = null)
    {
        var namedClient = name ?? "AuthorizeNetClient";

        services.AddSingleton<IAuthorizeNetClientOptions, AuthorizeNetClientOptions>(sp => new AuthorizeNetClientOptions(namedClient));

        services.AddChangeTokenOptions<AuthorizeNetOptions>(sectionName, configureAction: o => configureOptions?.Invoke(o));

        services.AddScoped(sp =>
        {
            var options = sp.GetRequiredService<IOptionsMonitor<AuthorizeNetOptions>>().CurrentValue;

            return new MerchantAuthenticationType
            {
                Name = options.ClientName,
                TransactionKey = options.TransactionKey,
            };
        });

        var httpClientBuilder = services.AddHttpClient(namedClient, (sp, configure) =>
        {
            var options = sp.GetRequiredService<IOptionsMonitor<AuthorizeNetOptions>>().CurrentValue;
            configure.BaseAddress = options.BaseUri;
        });

        // adds polly policy for retries etc. if needed.
        if (retryPolicy != null)
        {
            httpClientBuilder.AddPolicyHandler(retryPolicy);
        }

        services.TryAddTransient<IAuthorizeNetClient<CreateCustomerProfileRequest, CreateCustomerProfileResponse>, AuthorizeNetClient<CreateCustomerProfileRequest, CreateCustomerProfileResponse>>();
        services.TryAddTransient<IAuthorizeNetClient<CreateCustomerProfileFromTransactionRequest, CreateCustomerProfileResponse>, AuthorizeNetClient<CreateCustomerProfileFromTransactionRequest, CreateCustomerProfileResponse>>();

        services.TryAddTransient<IAuthorizeNetClient<GetCustomerProfileRequest, GetCustomerProfileResponse>, AuthorizeNetClient<GetCustomerProfileRequest, GetCustomerProfileResponse>>();
        services.TryAddTransient<IAuthorizeNetClient<GetCustomerProfileIdsRequest, GetCustomerProfileIdsResponse>, AuthorizeNetClient<GetCustomerProfileIdsRequest, GetCustomerProfileIdsResponse>>();

        services.TryAddTransient<IAuthorizeNetClient<DeleteCustomerProfileRequest, DeleteCustomerProfileResponse>, AuthorizeNetClient<DeleteCustomerProfileRequest, DeleteCustomerProfileResponse>>();
        services.TryAddTransient<IAuthorizeNetClient<UpdateCustomerProfileRequest, UpdateCustomerProfileResponse>, AuthorizeNetClient<UpdateCustomerProfileRequest, UpdateCustomerProfileResponse>>();

        services.TryAddTransient<ICustomerProfileClient, CustomerProfileClient>();

        services.TryAddTransient<IAuthorizeNetClient<CreateCustomerPaymentProfileRequest, CreateCustomerPaymentProfileResponse>, AuthorizeNetClient<CreateCustomerPaymentProfileRequest, CreateCustomerPaymentProfileResponse>>();
        services.TryAddTransient<IAuthorizeNetClient<GetCustomerPaymentProfileRequest, GetCustomerPaymentProfileResponse>, AuthorizeNetClient<GetCustomerPaymentProfileRequest, GetCustomerPaymentProfileResponse>>();
        services.TryAddTransient<IAuthorizeNetClient<GetCustomerPaymentProfileListRequest, GetCustomerPaymentProfileListResponse>, AuthorizeNetClient<GetCustomerPaymentProfileListRequest, GetCustomerPaymentProfileListResponse>>();
        services.TryAddTransient<IAuthorizeNetClient<UpdateCustomerPaymentProfileRequest, UpdateCustomerPaymentProfileResponse>, AuthorizeNetClient<UpdateCustomerPaymentProfileRequest, UpdateCustomerPaymentProfileResponse>>();
        services.TryAddTransient<IAuthorizeNetClient<DeleteCustomerPaymentProfileRequest, DeleteCustomerPaymentProfileResponse>, AuthorizeNetClient<DeleteCustomerPaymentProfileRequest, DeleteCustomerPaymentProfileResponse>>();
        services.TryAddTransient<IAuthorizeNetClient<ValidateCustomerPaymentProfileRequest, ValidateCustomerPaymentProfileResponse>, AuthorizeNetClient<ValidateCustomerPaymentProfileRequest, ValidateCustomerPaymentProfileResponse>>();

        services.TryAddTransient<ICustomerPaymentProfileClient, CustomerPaymentProfileClient>();

        services.TryAddTransient<IAuthorizeNetClient<CreateCustomerShippingAddressRequest, CreateCustomerShippingAddressResponse>, AuthorizeNetClient<CreateCustomerShippingAddressRequest, CreateCustomerShippingAddressResponse>>();
        services.TryAddTransient<IAuthorizeNetClient<GetCustomerShippingAddressRequest, GetCustomerShippingAddressResponse>, AuthorizeNetClient<GetCustomerShippingAddressRequest, GetCustomerShippingAddressResponse>>();
        services.TryAddTransient<IAuthorizeNetClient<UpdateCustomerShippingAddressRequest, UpdateCustomerShippingAddressResponse>, AuthorizeNetClient<UpdateCustomerShippingAddressRequest, UpdateCustomerShippingAddressResponse>>();
        services.TryAddTransient<IAuthorizeNetClient<DeleteCustomerShippingAddressRequest, DeleteCustomerShippingAddressResponse>, AuthorizeNetClient<DeleteCustomerShippingAddressRequest, DeleteCustomerShippingAddressResponse>>();

        services.TryAddTransient<ICustomerShippingAddressClient, CustomerShippingAddressClient>();

        services.TryAddTransient<IAuthorizeNetClient<CreateTransactionRequest, CreateTransactionResponse>, AuthorizeNetClient<CreateTransactionRequest, CreateTransactionResponse>>();
        services.TryAddTransient<IAuthorizeNetClient<GetTransactionDetailsRequest, GetTransactionDetailsResponse>, AuthorizeNetClient<GetTransactionDetailsRequest, GetTransactionDetailsResponse>>();
        services.TryAddTransient<IAuthorizeNetClient<GetSettledBatchListRequest, GetSettledBatchListResponse>, AuthorizeNetClient<GetSettledBatchListRequest, GetSettledBatchListResponse>>();
        services.TryAddTransient<IAuthorizeNetClient<GetTransactionListRequest, GetTransactionListResponse>, AuthorizeNetClient<GetTransactionListRequest, GetTransactionListResponse>>();
        services.TryAddTransient<IAuthorizeNetClient<GetUnsettledTransactionListRequest, GetUnsettledTransactionListResponse>, AuthorizeNetClient<GetUnsettledTransactionListRequest, GetUnsettledTransactionListResponse>>();

        services.TryAddTransient<ITransactionClient, TransactionClient>();

        return services;
    }
}
