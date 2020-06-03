using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

using AuthorizeNet.Api.V1.Contracts;

using Bet.Extensions.AuthorizeNet.Api.V1.Clients;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AuthorizeNet.Worker
{
    public class Worker : BackgroundService
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<Worker> _logger;

        public Worker(IServiceProvider provider, ILogger<Worker> logger)
        {
            _provider = provider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _provider.CreateScope();

                var customerProfileClient = scope.ServiceProvider
                    .GetRequiredService<CustomerProfileClient>();

                var profiles = new Collection<CustomerPaymentProfileType>
                {
                    new CustomerPaymentProfileType
                    {
                        CustomerType = CustomerTypeEnum.Business,
                        Payment = new PaymentType
                        {
                            CreditCard = new CreditCardType
                            {
                                CardNumber = "4111111111111111",
                                ExpirationDate = "2020-12"
                            }
                        }
                    }
                };

                var request = new CreateCustomerProfileRequest
                {
                    Profile = new CustomerProfileType
                    {
                        Description = "Test Customer Account",
                        Email = "email2@email.com",
                        MerchantCustomerId = "CustomerId-2",
                        ProfileType = CustomerProfileTypeEnum.Regular,
                        PaymentProfiles = profiles,
                    },

                    ValidationMode = ValidationModeEnum.TestMode
                };

                var result = await customerProfileClient.CreateAsync(request, stoppingToken);

                var delResult = await customerProfileClient.DeleteAsync(new DeleteCustomerProfileRequest
                {
                    CustomerProfileId = "1512050990",
                    RefId = "ref1"
                });

                _logger.LogDebug(result.Messages.ResultCode.ToString());

                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(TimeSpan.FromSeconds(20), stoppingToken);
            }
        }
    }
}
