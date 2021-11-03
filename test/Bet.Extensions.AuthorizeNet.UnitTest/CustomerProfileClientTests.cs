using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using AuthorizeNet.Api.V1.Contracts;

using Bet.Extensions.AuthorizeNet.Api.V1.Clients;
using Bet.Extensions.AuthorizeNet.Api.V1.Contracts;
using Bet.Extensions.AuthorizeNet.Options;
using Bet.Extensions.Testing.Logging;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Xunit;
using Xunit.Abstractions;

namespace Bet.Extensions.AuthorizeNet.UnitTest
{
    public class CustomerProfileClientTests
    {
        private readonly ITestOutputHelper _output;
        private readonly IServiceProvider _sp;

        public CustomerProfileClientTests(ITestOutputHelper output)
        {
            _output = output;
            _sp = GetServiceProvider(output);
        }

        [Fact]
        public async Task Create_Get_Delete_Customer_Profile_With_Credit_Card_Successfully_Test()
        {
            // Arrange
            var client = _sp.GetRequiredService<ICustomerProfileClient>();
            var paymentClient = _sp.GetRequiredService<ICustomerPaymentProfileClient>();
            var transactionClient = _sp.GetRequiredService<ITransactionClient>();
            var options = _sp.GetRequiredService<IOptions<AuthorizeNetOptions>>();

            // 1. Act create Customer/Payment profile
            var creditCardPaymentProfiles = new Collection<CustomerPaymentProfileType>
                {
                    new CustomerPaymentProfileType
                    {
                        CustomerType = CustomerTypeEnum.Business,
                        Payment = new PaymentType
                        {
                            CreditCard = new CreditCardType
                            {
                                CardNumber = "4111111111111111",
                                ExpirationDate = "2024-12",
                                CardCode = "900"
                            }
                        },

                        // visa requires to have the address
                        BillTo = new CustomerAddressType
                        {
                            Address = "1234 main st",
                            City = "Washington",
                            State = "DC",
                            Zip = "46282"
                        },
                    }
                };

            // 1a. create request
            var createProfileRequest = new CreateCustomerProfileRequest
            {
                Profile = new CustomerProfileType
                {
                    Description = "Test Customer Account",
                    Email = "email2@email.com",
                    MerchantCustomerId = "CustomerId-2",
                    ProfileType = CustomerProfileTypeEnum.Regular,
                    PaymentProfiles = creditCardPaymentProfiles,
                },

                ValidationMode = options.Value.ValidationMode
            };

            var createResponse = await client.CreateAsync(createProfileRequest);

            // Code: E00039 Text: A duplicate record with ID 1517706258 already exists
            var code = createResponse.Messages.Message[0].Code;
            var text = createResponse.Messages.Message[0].Text;
            Assert.Equal("I00001", code);
            Assert.Equal("Successful.", text);

            var createResult = new PaymentGatewayResponse(createResponse.ValidationDirectResponseList[0]);

            Assert.Equal(MessageTypeEnum.Ok, createResponse.Messages.ResultCode);
            Assert.Equal(ResponseCodeEnum.Approved, createResult.ResponseCode);
            Assert.Equal("This transaction has been approved.", createResult.ResponseReasonCode);

            await Task.Delay(TimeSpan.FromSeconds(10));

            // 2. Act get Customer/Payment profiles by a customer email
            var getProfileResponse = await client.GetAsync(new GetCustomerProfileRequest
            {
                Email = "email2@email.com",

                // CustomerProfileId = createResponse.CustomerProfileId,
                UnmaskExpirationDate = true,
            });

            Assert.Equal(MessageTypeEnum.Ok, getProfileResponse.Messages.ResultCode);
            _output.WriteLine($"{getProfileResponse.Profile.CustomerProfileId} - {getProfileResponse.Messages.ResultCode}");

            // 3. Act get Customer Payment profile with unmasked values prep for an update
            var getPaymentResponse = await paymentClient.GetAsync(new GetCustomerPaymentProfileRequest
            {
                CustomerPaymentProfileId = getProfileResponse.Profile.PaymentProfiles[0].CustomerPaymentProfileId,
                CustomerProfileId = getProfileResponse.Profile.CustomerProfileId,
                UnmaskExpirationDate = true
            });

            Assert.Equal(MessageTypeEnum.Ok, getPaymentResponse.Messages.ResultCode);

            // 4. Act update Customer Payment profile
            var exp = "2025-10";

            var updatePaymentRequest = new UpdateCustomerPaymentProfileRequest
            {
                ValidationMode = options.Value.ValidationMode,

                CustomerProfileId = getPaymentResponse.PaymentProfile.CustomerProfileId,
                PaymentProfile = new CustomerPaymentProfileExType
                {
                    CustomerPaymentProfileId = getPaymentResponse.PaymentProfile.CustomerPaymentProfileId,
                    CustomerType = CustomerTypeEnum.Individual,
                    Payment = new PaymentType
                    {
                        CreditCard = new CreditCardType
                        {
                            CardNumber = getPaymentResponse.PaymentProfile.Payment.CreditCard.CardNumber,
                            ExpirationDate = exp,
                            CardCode = "900"
                        }
                    }
                }
            };

            _output.WriteLine($"Old: {getPaymentResponse.PaymentProfile.Payment.CreditCard.ExpirationDate}; New:{exp}");

            var updatePaymentResponse = await paymentClient.UpdateAsync(updatePaymentRequest);
            var updatePaymentResult = new PaymentGatewayResponse(updatePaymentResponse.ValidationDirectResponse);

            Assert.Equal(MessageTypeEnum.Ok, updatePaymentResponse.Messages.ResultCode);
            Assert.Equal(ResponseCodeEnum.Approved, updatePaymentResult.ResponseCode);

            await Task.Delay(TimeSpan.FromSeconds(10));

            // 5. Act get an Update Customer Payment profile
            var getUpdatedProfileResponse = await client.GetAsync(new GetCustomerProfileRequest
            {
                Email = "email2@email.com",
                UnmaskExpirationDate = true,
            });

            Assert.Equal(exp, getUpdatedProfileResponse.Profile.PaymentProfiles[0].Payment.CreditCard.ExpirationDate);

            // 5. Act Charge Customer Payment profile
            var chargeRequest = new CreateTransactionRequest
            {
                TransactionRequest = new TransactionRequestType
                {
                    Amount = 5.05m,
                    TransactionType = Enum.GetName(typeof(TransactionTypeEnum), TransactionTypeEnum.AuthCaptureTransaction),
                    Profile = new CustomerProfilePaymentType
                    {
                        CustomerProfileId = getUpdatedProfileResponse.Profile.CustomerProfileId,
                        PaymentProfile = new PaymentProfile
                        {
                            PaymentProfileId = getUpdatedProfileResponse.Profile.PaymentProfiles[0].CustomerPaymentProfileId
                        }
                    },

                    Customer = new CustomerDataType
                    {
                        Id = "profile-test-56789"
                    },
                    Order = new OrderType
                    {
                        InvoiceNumber = "cp-invoice-123"
                    },

                    CustomerIP = options.Value.IpAddress,
                }
            };

            var chargeResponse = await transactionClient.CreateAsync(chargeRequest);
            Assert.Equal(MessageTypeEnum.Ok, chargeResponse.Messages.ResultCode);
            Assert.Equal("1", chargeResponse.TransactionResponse.ResponseCode);
            Assert.Equal("This transaction has been approved.", chargeResponse.TransactionResponse.Messages[0].Description);
            _output.WriteLine(chargeResponse.TransactionResponse.TransId);

            // 7. Act get an Update Customer Payment profile
            var deleteRequest = new DeleteCustomerProfileRequest
            {
                CustomerProfileId = createResponse.CustomerProfileId
            };

            var deleteResponse = await client.DeleteAsync(deleteRequest);

            Assert.Equal(MessageTypeEnum.Ok, deleteResponse.Messages.ResultCode);
        }

        [Fact]
        public async Task Create_Get_Delete_Customer_Profile_With_eCheck_Business_Checking_Successfully_Test()
        {
            // Arrange
            var client = _sp.GetRequiredService<ICustomerProfileClient>();
            var paymentClient = _sp.GetRequiredService<ICustomerPaymentProfileClient>();
            var transactionClient = _sp.GetRequiredService<ITransactionClient>();
            var options = _sp.GetRequiredService<IOptions<AuthorizeNetOptions>>();

            var randomAccountNumber = new Random().Next(10000, int.MaxValue);

            // 1. Act create eCheck Customer/Payment profile
            var eCheckPaymentProfiles = new Collection<CustomerPaymentProfileType>
                {
                    // e-check business checking
                    new CustomerPaymentProfileType
                    {
                        CustomerType = CustomerTypeEnum.Business,
                        Payment = new PaymentType
                        {
                            BankAccount = new BankAccountType
                            {
                                BankName = "Bank of China",
                                AccountType = BankAccountTypeEnum.BusinessChecking,
                                RoutingNumber = "125008547",
                                AccountNumber = randomAccountNumber.ToString(),
                                NameOnAccount = "Joseph Stalin(Biden)", // .Substring(0, 22),
                                EcheckType = EcheckTypeEnum.CCD
                            },
                        }
                    },

                    new CustomerPaymentProfileType
                    {
                        CustomerType = CustomerTypeEnum.Business,
                        Payment = new PaymentType
                        {
                            CreditCard = new CreditCardType
                            {
                                CardNumber = "2223000010309703",
                                ExpirationDate = "2024-12",
                                CardCode = "900"
                            }
                        },

                        // visa requires to have the address
                        BillTo = new CustomerAddressType
                        {
                            Address = "1600 Pennsylvania Avenue NW",
                            City = "Washington",
                            State = "DC",
                            Zip = "20500"
                        },
                    }
                };

            // 1a. create request
            var createProfileRequest = new CreateCustomerProfileRequest
            {
                Profile = new CustomerProfileType
                {
                    Description = "eCheck Business Checking Customer",
                    Email = "echeck-business-checking@email.com",

                    // id within e-commerce site for the customer
                    MerchantCustomerId = "echeck-id-2",
                    ProfileType = CustomerProfileTypeEnum.Regular,
                    PaymentProfiles = eCheckPaymentProfiles,
                },

                ValidationMode = options.Value.ValidationMode
            };

            var createResponse = await client.CreateAsync(createProfileRequest);

            var code = createResponse.Messages.Message[0].Code;
            var text = createResponse.Messages.Message[0].Text;
            Assert.Equal("I00001", code);
            Assert.Equal("Successful.", text);
            var createResult = new PaymentGatewayResponse(createResponse.ValidationDirectResponseList[0]);

            Assert.Equal(MessageTypeEnum.Ok, createResponse.Messages.ResultCode);
            Assert.Equal(ResponseCodeEnum.Approved, createResult.ResponseCode);
            Assert.Equal("This transaction has been approved.", createResult.ResponseReasonCode);

            // delay for the time to process the record
            await Task.Delay(TimeSpan.FromSeconds(10));

            // 2. Act get an Customer/Payment profile
            var getUpdatedProfileResponse = await client.GetAsync(new GetCustomerProfileRequest
            {
                Email = "echeck1@email.com",
                UnmaskExpirationDate = true,
            });

            // 3. Act Charge Customer Payment profile
            // pay with e-check
            var paymentProfileId = getUpdatedProfileResponse
                                    .Profile
                                    .PaymentProfiles
                                    .FirstOrDefault(x => x.Payment.BankAccount.EcheckType == EcheckTypeEnum.CCD)?
                                    .CustomerPaymentProfileId;

            var chargeRequest = new CreateTransactionRequest
            {
                TransactionRequest = new TransactionRequestType
                {
                    Amount = 15.05m,
                    TransactionType = Enum.GetName(typeof(TransactionTypeEnum), TransactionTypeEnum.AuthCaptureTransaction),
                    Profile = new CustomerProfilePaymentType
                    {
                        CustomerProfileId = getUpdatedProfileResponse.Profile.CustomerProfileId,
                        PaymentProfile = new PaymentProfile
                        {
                            // pay with e-check
                            PaymentProfileId = paymentProfileId
                        }
                    },

                    Customer = new CustomerDataType
                    {
                        Id = "profile-test-56789"
                    },
                    Order = new OrderType
                    {
                        InvoiceNumber = "eck-invoice-1"
                    },

                    CustomerIP = options.Value.IpAddress,
                }
            };

            var chargeResponse = await transactionClient.CreateAsync(chargeRequest);
            Assert.Equal(MessageTypeEnum.Ok, chargeResponse.Messages.ResultCode);
            Assert.Equal(ResponseCodeEnum.Approved, ResponseMapper.GetResponseCode(chargeResponse.TransactionResponse.ResponseCode));
            Assert.Equal("This transaction has been approved.", chargeResponse.TransactionResponse.Messages[0].Description);
            _output.WriteLine(chargeResponse.TransactionResponse.TransId);

            // 4. Act get an Update Customer Payment profile
            var deleteRequest = new DeleteCustomerProfileRequest
            {
                CustomerProfileId = createResponse.CustomerProfileId
            };

            var deleteResponse = await client.DeleteAsync(deleteRequest);

            Assert.Equal(MessageTypeEnum.Ok, deleteResponse.Messages.ResultCode);
        }

        [Fact]
        public async Task Fail_To_Create_Customer_Profile_With_Credit_Card_That_Expired_Test()
        {
            var client = _sp.GetRequiredService<ICustomerProfileClient>();
            var options = _sp.GetRequiredService<IOptions<AuthorizeNetOptions>>();

            // customer payment profile
            var customerPaymentProfiles = new Collection<CustomerPaymentProfileType>
                {
                    new CustomerPaymentProfileType
                    {
                        CustomerType = CustomerTypeEnum.Business,
                        Payment = new PaymentType
                        {
                            CreditCard = new CreditCardType
                            {
                                CardNumber = "4111111111111111",
                                ExpirationDate = "2020-12",
                                CardCode = "900"
                            },
                        },
                    }
                };

            // create request
            var createRequest = new CreateCustomerProfileRequest
            {
                Profile = new CustomerProfileType
                {
                    Description = "Expired Test Customer Account",
                    Email = "expried@email.com",
                    MerchantCustomerId = "Expired-CustomerId-2",
                    ProfileType = CustomerProfileTypeEnum.Regular,
                    PaymentProfiles = customerPaymentProfiles,
                },

                ValidationMode = options.Value.ValidationMode
            };

            var createResponse = await client.CreateAsync(createRequest);
            Assert.Equal(MessageTypeEnum.Error, createResponse.Messages.ResultCode);

            var code = createResponse.Messages.Message[0].Code;
            var text = createResponse.Messages.Message[0].Text;
            Assert.Equal("E00027", code);
            Assert.Equal("The credit card has expired.", text);

            var createResult = new PaymentGatewayResponse(createResponse.ValidationDirectResponseList[0]);

            _output.WriteLine($"Account: [{createResult.AccountNumber}]; " +
                             $": [{createResult.ResponseCode}]");

            Assert.Equal("P", createResult.AVSResponseCode);
            Assert.Equal("auth_only", createResult.TransactionType);
            Assert.Equal("This transaction is an uknown state.", createResult.ResponseReasonCode);
            Assert.Equal("Visa", createResult.CardType);
            Assert.Equal(string.Empty, createResult.CardCodeResponse);
            Assert.Equal(string.Empty, createResult.AuthorizationCode);
            Assert.Equal(string.Empty, createResult.CardholderAuthenticationVerificationResponse);
            Assert.Equal(string.Empty, createResult.AuthorizationCode);
            Assert.Equal("CC", createResult.Method);
        }

        [Fact]
        public async Task Fail_To_Create_Customer_Profile_With_Credit_Card_That_Declined_Test()
        {
            // Arrange
            var client = _sp.GetRequiredService<ICustomerProfileClient>();
            var options = _sp.GetRequiredService<IOptions<AuthorizeNetOptions>>();

            // customer payment profile
            var customerPaymentProfiles = new Collection<CustomerPaymentProfileType>
                {
                    new CustomerPaymentProfileType
                    {
                        CustomerType = CustomerTypeEnum.Business,
                        Payment = new PaymentType
                        {
                            CreditCard = new CreditCardType
                            {
                                CardNumber = "5424000000000015",
                                ExpirationDate = "2024-02",
                                CardCode = "904",
                            }
                        },
                        BillTo = new CustomerAddressType
                        {
                            Address = "1234 main st",
                            City = "Washington",
                            State = "DC",
                            Zip = "46282"
                        },
                    }
                };

            // create request
            var createRequest = new CreateCustomerProfileRequest
            {
                Profile = new CustomerProfileType
                {
                    Description = "Declined Test Customer Account",
                    Email = "declined@email.com",
                    MerchantCustomerId = "Declined-Id-2",
                    ProfileType = CustomerProfileTypeEnum.Regular,
                    PaymentProfiles = customerPaymentProfiles,
                },

                ValidationMode = options.Value.ValidationMode
            };

            var createResponse = await client.CreateAsync(createRequest);

            // E00039 - A duplicate record with ID 1517706063 already exists.
            var code = createResponse.Messages.Message[0].Code;
            var text = createResponse.Messages.Message[0].Text;
            Assert.Equal("E00027", code);
            Assert.Equal("This transaction has been declined.", text);
            Assert.Equal(MessageTypeEnum.Error, createResponse.Messages.ResultCode);

            var createResult = new PaymentGatewayResponse(createResponse.ValidationDirectResponseList[0]);

            // Assert.Equal(ResponseCodeEnum.HeldForReview, createResult.ResponseCode);
            Assert.Equal(ResponseCodeEnum.Declined, createResult.ResponseCode);

            // "N" => "No Match on Address(Street) or ZIP"
            Assert.Equal("Z", createResult.AVSResponseCode);
            Assert.Equal("Five digit ZIP matches, Address (Street) does not", createResult.AVSResponseText);

            // Assert.Equal("A", createResult.AVSResponseCode);
            // Assert.Equal("Address(Street) matches, ZIP does not", createResult.AVSResponseText);

            // good address match
            // Assert.Equal("Y", createResult.AVSResponseCode);
            // Assert.Equal("Address(Street) and five digit ZIP match", createResult.AVSResponseText);
            Assert.Equal("auth_only", createResult.TransactionType);
            Assert.Equal("This transaction has been declined", createResult.ResponseReasonCode);
            Assert.Equal("Visa", createResult.CardType);

            // N - No Match indicates the code entered is incorrect.
            Assert.Equal("N", createResult.CardCodeResponse);
            Assert.Equal("000000", createResult.AuthorizationCode);
            Assert.Equal(string.Empty, createResult.CardholderAuthenticationVerificationResponse);
            Assert.Equal("CC", createResult.Method);
        }

        [Fact]
        public void Test_DirectResponse()
        {
            // customer profile returns this as string
            var directResponse = @"2,2,27,The transaction has been declined because of an AVS mismatch. The address provided does not match billing address of cardholder.,AE5V2W,N,40059863807,none,Test transaction for ValidateCustomerPaymentProfile.,0.00,CC,auth_only,customer-48,Verdie,Farrell,Schimmel, McGlynn and Kling,214 Lucinda Streets,West Barrett,Kansas,46201,TC,,,Verdie_Farrell@gmail.com,,,,,,,,,0.00,0.00,0.00,FALSE,none,,P,2,,,,,,,,,,,XXXX0015,MasterCard,,,,,,,0S8C3LQM3HC6DEDAFV94OTO,,,,,,,,,,";

            var parsed = new PaymentGatewayResponse(directResponse);

            Assert.Equal(ResponseCodeEnum.Declined, parsed.ResponseCode);
        }

        private IServiceProvider GetServiceProvider(ITestOutputHelper output)
        {
            var services = new ServiceCollection();
            var configuration = TestConfigurations.GetConfiguration();

            services.AddSingleton(configuration);

            services.AddLogging(b => b.AddXunit(output));

            services.AddAuthorizeNet();
            return services.BuildServiceProvider();
        }
    }
}
