using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

using AuthorizeNet.Api.V1.Contracts;

using Bet.Extensions.AuthorizeNet.Api.V1.Clients;
using Bet.Extensions.AuthorizeNet.Options;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AuthorizeNet.Worker.Services
{
    public class CustomerService
    {
        private readonly AuthorizeNetOptions _options;
        private readonly ICustomerProfileClient _customerProfileClient;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(
            IOptions<AuthorizeNetOptions> options,
            ICustomerProfileClient customerProfileClient,
            ILogger<CustomerService> logger)
        {
            _options = options.Value;
            _customerProfileClient = customerProfileClient ?? throw new ArgumentNullException(nameof(customerProfileClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task TestCustomerProfileAsync(CancellationToken cancellationToken)
        {
            var refId = new Random(1000).Next().ToString();

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
                                ExpirationDate = "2021-12"
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

                ValidationMode = _options.IsSandBox ? ValidationModeEnum.TestMode : ValidationModeEnum.LiveMode,
                RefId = refId,
            };

            var result = await _customerProfileClient.CreateAsync(request, cancellationToken);
            _logger.LogInformation("Created Customer/Payment Profile: {code}", result.Messages.ResultCode.ToString());

            var delResult = await _customerProfileClient.DeleteAsync(new DeleteCustomerProfileRequest
            {
                CustomerProfileId = result.CustomerProfileId,
                RefId = refId
            });

            _logger.LogInformation("Deleted Customer/Payment Profile: {code}", delResult.Messages.ResultCode.ToString());
        }
    }
}
