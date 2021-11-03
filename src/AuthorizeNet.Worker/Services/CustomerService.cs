using System.Collections.ObjectModel;

using AuthorizeNet.Api.V1.Contracts;
using AuthorizeNet.Worker.Models;

using Bet.Extensions.AuthorizeNet.Api.V1.Clients;
using Bet.Extensions.AuthorizeNet.Api.V1.Contracts;
using Bet.Extensions.AuthorizeNet.Options;

using Microsoft.Extensions.Options;

namespace AuthorizeNet.Worker.Services;

public class CustomerService
{
    private readonly AuthorizeNetOptions _options;
    private readonly ICustomerProfileClient _customerProfileClient;
    private readonly ICustomerPaymentProfileClient _customerPaymentProfileClient;
    private readonly SampleData _sampleData;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(
        IOptions<AuthorizeNetOptions> options,
        ICustomerProfileClient customerProfileClient,
        ICustomerPaymentProfileClient customerPaymentProfileClient,
        SampleData sampleData,
        ILogger<CustomerService> logger)
    {
        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        _options = options.Value;
        _customerProfileClient = customerProfileClient ?? throw new ArgumentNullException(nameof(customerProfileClient));
        _customerPaymentProfileClient = customerPaymentProfileClient ?? throw new ArgumentNullException(nameof(customerPaymentProfileClient));
        _sampleData = sampleData;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task TestCustomerProfileAsync(CustomerProfile profile, CancellationToken cancellationToken)
    {
        // 1. create customer / payment profile with validation enabled.
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
            RefId = profile.ReferenceId,
            ValidationMode = _options.ValidationMode,
            Profile = new CustomerProfileType
            {
                Description = profile.Description,
                Email = profile.Email,
                MerchantCustomerId = profile.CustomerId,
                ProfileType = profile.CustomerProfileType,
                PaymentProfiles = paymentProfiles,
            },
        };

        var createResponse = await _customerProfileClient.CreateAsync(createRequest, cancellationToken);

        // validation list is in the same order as it has been submitted by the client code
        var parsedCreation = new PaymentGatewayResponse(createResponse.ValidationDirectResponseList[0]);

        // creation for the profile was successful
        if (createResponse.Messages.ResultCode == MessageTypeEnum.Ok)
        {
            var customerProfileId = createResponse.CustomerProfileId;
            var customerPaymentProfileId = createResponse.CustomerPaymentProfileIdList[0];

            _logger.LogInformation(
                "CreateResponse - {customerProfileId} - {paymentProfile} - {asvCode}",
                customerProfileId,
                createResponse.CustomerPaymentProfileIdList[0],
                parsedCreation.AVSResponseText);

            DisplayResponse("CreateResponse", createResponse);

            // 2. create another payment profile
            var secondaryProfile = _sampleData.GetCustomerProfiles()[1];

            var secondaryProfileRequest = new CreateCustomerPaymentProfileRequest
            {
                RefId = profile.ReferenceId,
                ValidationMode = _options.ValidationMode,
                CustomerProfileId = customerProfileId,
                PaymentProfile = new CustomerPaymentProfileType
                {
                    Payment = new PaymentType
                    {
                        CreditCard = new CreditCardType
                        {
                            CardCode = secondaryProfile.CardCode,
                            CardNumber = secondaryProfile.CardNumber,
                            ExpirationDate = secondaryProfile.ExpirationDate,
                        }
                    },
                    CustomerType = CustomerTypeEnum.Business,
                    BillTo = new CustomerAddressType
                    {
                        FirstName = secondaryProfile.FirstName,
                        LastName = secondaryProfile.LastName,
                        Address = secondaryProfile.StreetLine,
                        Company = secondaryProfile.Company,
                        City = secondaryProfile.City,
                        State = secondaryProfile.StateOrProvice,
                        Zip = secondaryProfile.ZipCode,
                        Country = secondaryProfile.Country
                    }
                }
            };

            var secondaryPaymentResponse = await _customerPaymentProfileClient.CreateAsync(secondaryProfileRequest, cancellationToken);
            var secondaryProfileId = secondaryPaymentResponse.CustomerPaymentProfileId;

            if (secondaryPaymentResponse.Messages.ResultCode == MessageTypeEnum.Ok)
            {
                var validateRequest = new ValidateCustomerPaymentProfileRequest
                {
                    CustomerPaymentProfileId = secondaryProfileId,
                    CustomerProfileId = customerProfileId,
                    ValidationMode = _options.ValidationMode,
                    RefId = profile.ReferenceId
                };

                var validateResponse = await _customerPaymentProfileClient.ValidateAsync(validateRequest, cancellationToken);
                var parsedValidation = new PaymentGatewayResponse(validateResponse?.DirectResponse!);

                if (validateResponse?.Messages.ResultCode == MessageTypeEnum.Error)
                {
                    _logger.LogWarning("{cardNumber}-{expDate}-{ccv}-{zip}-{parsed}", profile.CardNumber, profile.ExpirationDate, profile.CardCode, profile.ZipCode, parsedValidation.ResponseReasonText);
                    _logger.LogWarning(validateResponse.DirectResponse);
                }

                DisplayResponse("ValidationResponse", validateResponse!);

                // get customer profile
                var customerRequest = new GetCustomerProfileRequest
                {
                    CustomerProfileId = customerProfileId
                };

                var customerResponse = await _customerProfileClient.GetAsync(customerRequest, cancellationToken);
                DisplayResponse(" GetCustomerProfileResponse", customerResponse);

                var paymentRequest = new GetCustomerPaymentProfileRequest
                {
                    CustomerPaymentProfileId = customerPaymentProfileId,
                    CustomerProfileId = customerProfileId,
                    UnmaskExpirationDate = true
                };

                var paymentResponse = await _customerPaymentProfileClient.GetAsync(paymentRequest, cancellationToken);
                DisplayResponse(" GetCustomerPaymentProfileResponse", paymentResponse);
            }
        }

        _logger.LogWarning("CreateResponse - {responseCode}", parsedCreation.ResponseCode);

        // delete
        var deleteResponse = await _customerProfileClient.DeleteAsync(
            new DeleteCustomerProfileRequest
            {
                CustomerProfileId = createResponse.CustomerProfileId,
                RefId = profile.ReferenceId
            },
            cancellationToken);
        DisplayResponse("DeleteResponse", deleteResponse);
    }

    private void DisplayResponse(string action, ANetApiResponse response)
    {
        _logger.LogInformation("{action} - {code}", action, response?.Messages?.ResultCode.ToString());
        _logger.LogInformation("{action} - Code:{code}; Text:{text}", action, response?.Messages?.Message[0].Code, response?.Messages?.Message[0].Text);
    }
}
