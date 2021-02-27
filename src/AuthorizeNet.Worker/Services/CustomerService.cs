using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

using AuthorizeNet.Api.V1.Contracts;
using AuthorizeNet.Worker.Models;

using Bet.Extensions.AuthorizeNet.Api.V1.Clients;
using Bet.Extensions.AuthorizeNet.Api.V1.Contracts;
using Bet.Extensions.AuthorizeNet.Options;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AuthorizeNet.Worker.Services
{
    public class CustomerService
    {
        private readonly AuthorizeNetOptions _options;
        private readonly ICustomerProfileClient _customerProfileClient;
        private readonly ICustomerPaymentProfileClient _customerPaymentProfileClient;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(
            IOptions<AuthorizeNetOptions> options,
            ICustomerProfileClient customerProfileClient,
            ICustomerPaymentProfileClient customerPaymentProfileClient,
            ILogger<CustomerService> logger)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _options = options.Value;
            _customerProfileClient = customerProfileClient ?? throw new ArgumentNullException(nameof(customerProfileClient));
            _customerPaymentProfileClient = customerPaymentProfileClient ?? throw new ArgumentNullException(nameof(customerPaymentProfileClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task TestCustomerProfileAsync(CustomerProfile profile, CancellationToken cancellationToken)
        {
            var validationMode = _options.IsSandBox ? ValidationModeEnum.TestMode : ValidationModeEnum.LiveMode;

            // 1. get customer id
            var paymentProfiles = new Collection<CustomerPaymentProfileType>
            {
                new CustomerPaymentProfileType
                {
                    CustomerType = profile.CustomerType,
                    Payment = new PaymentType
                    {
                        CreditCard = new CreditCardType
                        {
                            CardNumber = profile.CardNumber,
                            ExpirationDate = profile.ExpirationDate,
                            CardCode = profile.CardCode
                        }
                    },
                    BillTo = new CustomerAddressType
                    {
                         FirstName = profile.FirstName,
                         LastName = profile.LastName,
                         Address = profile.StreetLine,
                         Company = profile.Company,
                         City = profile.City,
                         State = profile.StateOrProvice,
                         Zip = profile.ZipCode,
                         Country = profile.Country
                    }
                }
            };

            var createRequest = new CreateCustomerProfileRequest
            {
                Profile = new CustomerProfileType
                {
                    Description = profile.Description,
                    Email = profile.Email,
                    MerchantCustomerId = profile.CustomerId,
                    ProfileType = profile.CustomerProfileType,
                    PaymentProfiles = paymentProfiles,
                },

                ValidationMode = validationMode,
                RefId = profile.ReferenceId,
            };

            // create
            var createResponse = await _customerProfileClient.CreateAsync(createRequest, cancellationToken);

            var customerProfileId = createResponse.CustomerProfileId;
            var customerPaymentProfileId = createResponse.CustomerPaymentProfileIdList[0];

            _logger.LogInformation("CreateResponse - {customerProfileId} - {paymentProfile}", customerProfileId, createResponse.CustomerPaymentProfileIdList[0]);
            DisplayResponse("CreateResponse", createResponse);

            // validate
            if (createResponse.Messages.ResultCode == MessageTypeEnum.Ok)
            {
                var validateRequest = new ValidateCustomerPaymentProfileRequest
                {
                    CustomerPaymentProfileId = customerPaymentProfileId,
                    CustomerProfileId = customerProfileId,
                    ValidationMode = ValidationModeEnum.LiveMode,
                    RefId = profile.ReferenceId
                };

                var validateResponse = await _customerPaymentProfileClient.ValidateAsync(validateRequest, cancellationToken);
                var parsedValidation = new PaymentGatewayResponse(validateResponse.DirectResponse);

                if (validateResponse.Messages.ResultCode == MessageTypeEnum.Error)
                {
                    _logger.LogWarning("{cardNumber}-{expDate}-{ccv}-{zip}", profile.CardNumber, profile.ExpirationDate, profile.CardCode, profile.ZipCode);
                    _logger.LogWarning(validateResponse.DirectResponse);
                }

                DisplayResponse("ValidationResponse", validateResponse);
            }

            // delete
            var deleteResponse = await _customerProfileClient.DeleteAsync(new DeleteCustomerProfileRequest
            {
                CustomerProfileId = createResponse.CustomerProfileId,
                RefId = profile.ReferenceId
            });

            DisplayResponse("DeleteResponse", deleteResponse);
        }

        public async Task ValidateCustomerProfileAsync(CancellationToken cancellationToken)
        {
            var request = new ValidateCustomerPaymentProfileRequest
            {
                CustomerPaymentProfileId = "1234567",
                CustomerProfileId = "910111213",
                ValidationMode = ValidationModeEnum.LiveMode,
                RefId = "123"
            };

            var response = await _customerPaymentProfileClient.ValidateAsync(request, cancellationToken);

            var error = response?.Messages?.ResultCode == MessageTypeEnum.Error ? "Error" : "Success";
            var message = $"Code: {response.Messages.Message[0].Code} Text: {response.Messages.Message[0].Text}";

            _logger.LogInformation("Payment Profile {error} with: {message}", error, message);
        }

        private void DisplayResponse(string action, ANetApiResponse response)
        {
            _logger.LogInformation("{action} - {code}", action, response?.Messages?.ResultCode.ToString());
            _logger.LogInformation("{action} - Code:{code}; Text:{text}", action, response?.Messages?.Message[0].Code, response?.Messages?.Message[0].Text);
        }
    }
}
