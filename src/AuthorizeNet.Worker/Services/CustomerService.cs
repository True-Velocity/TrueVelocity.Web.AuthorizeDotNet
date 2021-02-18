using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

using AuthorizeNet.Api.V1.Contracts;

using Bet.Extensions.AuthorizeNet.Api.V1.Clients;

using Microsoft.Extensions.Logging;

namespace AuthorizeNet.Worker.Services
{
    public class CustomerService
    {
        private readonly ICustomerProfileClient _customerProfileClient;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(
            ICustomerProfileClient customerProfileClient,
            ILogger<CustomerService> logger)
        {
            _customerProfileClient = customerProfileClient;
            _logger = logger;
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

                ValidationMode = ValidationModeEnum.TestMode,
                RefId = refId
            };

            var result = await _customerProfileClient.CreateAsync(request, cancellationToken);
            _logger.LogInformation("Created: {code}", result.Messages.ResultCode.ToString());

            var delResult = await _customerProfileClient.DeleteAsync(new DeleteCustomerProfileRequest
            {
                CustomerProfileId = result.CustomerProfileId,
                RefId = refId
            });

            _logger.LogInformation("Deleted: {code}", delResult.Messages.ResultCode.ToString());
        }
    }
}
