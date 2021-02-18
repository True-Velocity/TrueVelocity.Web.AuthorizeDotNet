using System;

using AuthorizeNet.Api.V1.Contracts;

using Bet.Extensions.AuthorizeNet.Api.V1.Clients;
using Bet.Extensions.AuthorizeNet.Options;

using Microsoft.Extensions.DependencyInjection.Extensions;
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
                configure.BaseAddress = options.BaseUri;
            });

            services.TryAddTransient<IAuthorizeNetClient<CreateCustomerProfileRequest, CreateCustomerProfileResponse>, AuthorizeNetClient<CreateCustomerProfileRequest, CreateCustomerProfileResponse>>();
            services.TryAddTransient<IAuthorizeNetClient<GetCustomerProfileRequest, GetCustomerProfileResponse>, AuthorizeNetClient<GetCustomerProfileRequest, GetCustomerProfileResponse>>();
            services.TryAddTransient<IAuthorizeNetClient<DeleteCustomerProfileRequest, DeleteCustomerProfileResponse>, AuthorizeNetClient<DeleteCustomerProfileRequest, DeleteCustomerProfileResponse>>();

            services.TryAddTransient<ICustomerProfileClient, CustomerProfileClient>();

            services.TryAddTransient<IAuthorizeNetClient<CreateCustomerPaymentProfileRequest, CreateCustomerPaymentProfileResponse>, AuthorizeNetClient<CreateCustomerPaymentProfileRequest, CreateCustomerPaymentProfileResponse>>();
            services.TryAddTransient<IAuthorizeNetClient<GetCustomerPaymentProfileRequest, GetCustomerPaymentProfileResponse>, AuthorizeNetClient<GetCustomerPaymentProfileRequest, GetCustomerPaymentProfileResponse>>();
            services.TryAddTransient<IAuthorizeNetClient<GetCustomerPaymentProfileListRequest, GetCustomerPaymentProfileListResponse>, AuthorizeNetClient<GetCustomerPaymentProfileListRequest, GetCustomerPaymentProfileListResponse>>();
            services.TryAddTransient<IAuthorizeNetClient<UpdateCustomerPaymentProfileRequest, UpdateCustomerPaymentProfileResponse>, AuthorizeNetClient<UpdateCustomerPaymentProfileRequest, UpdateCustomerPaymentProfileResponse>>();
            services.TryAddTransient<IAuthorizeNetClient<DeleteCustomerPaymentProfileRequest, DeleteCustomerPaymentProfileResponse>, AuthorizeNetClient<DeleteCustomerPaymentProfileRequest, DeleteCustomerPaymentProfileResponse>>();
            services.TryAddTransient<IAuthorizeNetClient<ValidateCustomerPaymentProfileRequest, ValidateCustomerPaymentProfileResponse>, AuthorizeNetClient<ValidateCustomerPaymentProfileRequest, ValidateCustomerPaymentProfileResponse>>();

            services.TryAddTransient<ICustomerPaymentProfileClient, CustomerPaymentProfileClient>();

            services.TryAddTransient<IAuthorizeNetClient<CreateTransactionRequest, CreateTransactionResponse>, AuthorizeNetClient<CreateTransactionRequest, CreateTransactionResponse>>();
            services.TryAddTransient<IAuthorizeNetClient<GetTransactionDetailsRequest, GetTransactionDetailsResponse>, AuthorizeNetClient<GetTransactionDetailsRequest, GetTransactionDetailsResponse>>();
            services.TryAddTransient<IAuthorizeNetClient<GetSettledBatchListRequest, GetSettledBatchListResponse>, AuthorizeNetClient<GetSettledBatchListRequest, GetSettledBatchListResponse>>();
            services.TryAddTransient<IAuthorizeNetClient<GetTransactionListRequest, GetTransactionListResponse>, AuthorizeNetClient<GetTransactionListRequest, GetTransactionListResponse>>();

            services.TryAddTransient<ITransactionClient, TransactionClient>();

            return services;
        }
    }
}
