using System;
using System.Linq;
using System.Threading.Tasks;

using AuthorizeNet.Api.V1.Contracts;

using Bet.Extensions.AuthorizeNet.Api.V1.Clients;
using Bet.Extensions.AuthorizeNet.Options;
using Bet.Extensions.Testing.Logging;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Xunit;
using Xunit.Abstractions;

namespace Bet.Extensions.AuthorizeNet.UnitTest
{
    public class TransactionClientTests
    {
        private readonly ITestOutputHelper _output;
        private readonly IConfiguration _configuration;

        public TransactionClientTests(ITestOutputHelper output)
        {
            _output = output;
            _configuration = TestConfigurations.GetConfiguration();
        }

        [Fact]
        public async Task Personal_Checking_AuthCapture_Successfully_Debit_Test()
        {
            var services = new ServiceCollection();

            services.AddSingleton(_configuration);
            services.AddLogging(b => b.AddXunit(_output));
            services.AddAuthorizeNet();
            var sp = services.BuildServiceProvider();

            var client = sp.GetRequiredService<ITransactionClient>();
            var options = sp.GetRequiredService<IOptions<AuthorizeNetOptions>>();

            var randomAccountNumber = new Random().Next(10000, int.MaxValue);

            var request = new CreateTransactionRequest
            {
                TransactionRequest = new TransactionRequestType
                {
                    TransactionType = Enum.GetName(typeof(TransactionTypeEnum), TransactionTypeEnum.AuthCaptureTransaction),
                    Amount = 13.59m, // 333.01m, to test failed transaction
                    Payment = new PaymentType
                    {
                        BankAccount = new BankAccountType
                        {
                            BankName = "Bank of USSR",
                            AccountType = BankAccountTypeEnum.Checking,
                            RoutingNumber = "125008547",
                            AccountNumber = randomAccountNumber.ToString(),

                            // CheckNumber = "",
                            NameOnAccount = "Brezhnev",
                            EcheckType = EcheckTypeEnum.WEB
                        },
                    },
                    CustomerIP = options.Value.IpAddress,
                    Order = new OrderType
                    {
                        InvoiceNumber = "PC-Invoice-456",
                        Description = "Personal Checking e-Check purchase"
                    },
                },
            };

            var response = await client.CreateAsync(request);
            _output.WriteLine($"{response.TransactionResponse.ResponseCode} - {response.TransactionResponse.TransId}");

            Assert.Equal(MessageTypeEnum.Ok, response.Messages.ResultCode);

            var code = response.Messages.Message[0].Code;
            var text = response.Messages.Message[0].Text;
            Assert.Equal("I00001", code);
            Assert.Equal("Successful.", text);

            Assert.False(response.TransactionResponse.Errors.Any());

            // if errors exist messages are 0
            var tcode = response.TransactionResponse.Messages[0].Code;
            var ttext = response.TransactionResponse.Messages[0].Description;
            Assert.Equal("1", tcode);
            Assert.Equal("This transaction has been approved.", ttext);

            // 1 success
            Assert.Equal("1", response.TransactionResponse.ResponseCode);
            Assert.True(!string.IsNullOrEmpty(response.TransactionResponse.TransId));
        }

        [Fact]
        public async Task Business_Checking_AuthCapture_Successfully_Debit_Test()
        {
            var services = new ServiceCollection();

            services.AddSingleton(_configuration);
            services.AddLogging(b => b.AddXunit(_output));
            services.AddAuthorizeNet();
            var sp = services.BuildServiceProvider();

            var client = sp.GetRequiredService<ITransactionClient>();
            var options = sp.GetRequiredService<IOptions<AuthorizeNetOptions>>();

            var randomAccountNumber = new Random().Next(10000, int.MaxValue);

            var request = new CreateTransactionRequest
            {
                TransactionRequest = new TransactionRequestType
                {
                    TransactionType = Enum.GetName(typeof(TransactionTypeEnum), TransactionTypeEnum.AuthCaptureTransaction),
                    Amount = 13.01m,
                    Payment = new PaymentType
                    {
                        BankAccount = new BankAccountType
                        {
                            BankName = "Bank of China",
                            AccountType = BankAccountTypeEnum.BusinessChecking,
                            RoutingNumber = "125008547",
                            AccountNumber = randomAccountNumber.ToString(),

                            // CheckNumber = "",
                            NameOnAccount = "Biden",
                            EcheckType = EcheckTypeEnum.CCD
                        },
                    },
                    CustomerIP = options.Value.IpAddress,
                    Order = new OrderType
                    {
                        InvoiceNumber = "BC-Invoice-789",
                        Description = "Business Checking e-Check purchase"
                    },
                },
            };

            var response = await client.CreateAsync(request);
            _output.WriteLine($"{response.TransactionResponse.ResponseCode} - {response.TransactionResponse.TransId}");

            Assert.Equal(MessageTypeEnum.Ok, response.Messages.ResultCode);

            var code = response.Messages.Message[0].Code;
            var text = response.Messages.Message[0].Text;
            Assert.Equal("I00001", code);
            Assert.Equal("Successful.", text);

            var tcode = response.TransactionResponse.Messages[0].Code;
            var ttext = response.TransactionResponse.Messages[0].Description;
            Assert.Equal("1", tcode);
            Assert.Equal("This transaction has been approved.", ttext);

            // 1 success
            Assert.Equal("1", response.TransactionResponse.ResponseCode);
            Assert.True(!string.IsNullOrEmpty(response.TransactionResponse.TransId));
            Assert.False(response.TransactionResponse.Errors.Any());
        }

        [Fact]
        public async Task Credit_Card_Charge_Declined_ZipCode_46282_Test()
        {
            var services = new ServiceCollection();
            services.AddSingleton(_configuration);
            services.AddLogging(b => b.AddXunit(_output));
            services.AddAuthorizeNet();
            var sp = services.BuildServiceProvider();

            var client = sp.GetRequiredService<ITransactionClient>();
            var options = sp.GetRequiredService<IOptions<AuthorizeNetOptions>>();

            var request = new CreateTransactionRequest
            {
                TransactionRequest = new TransactionRequestType
                {
                    CustomerIP = options.Value.IpAddress,
                    TransactionType = Enum.GetName(typeof(TransactionTypeEnum), TransactionTypeEnum.AuthOnlyTransaction),
                    Amount = 1.0m,
                    Payment = new PaymentType
                    {
                        CreditCard = new CreditCardType
                        {
                            CardNumber = "5424000000000015",
                            ExpirationDate = "2021-12",
                            CardCode = "901" // N   Does not match.
                        },
                    },
                    Customer = new CustomerDataType
                    {
                        Id = "cc-tester-56789"
                    },
                    Order = new OrderType
                    {
                        InvoiceNumber = "cc-invoice-123"
                    },
                    BillTo = new CustomerAddressType
                    {
                        Address = "1600 Pennsylvania Avenue NW",
                        City = "Washington",
                        State = "DC",
                        Zip = "46282" // causes decline in the card
                    }
                },
            };

            var response = await client.CreateAsync(request);

            _output.WriteLine($"Account:{response.TransactionResponse.AccountNumber}" +
                $"TransId: [{response.TransactionResponse.TransId}]; " +
                $"CVV: [{response.TransactionResponse.CvvResultCode}]; " +
                $"AVS: [{response.TransactionResponse.AvsResultCode}]; " +
                $"CAVV: [{response.TransactionResponse.CavvResultCode}]");

            var code = response.Messages.Message[0].Code;
            var text = response.Messages.Message[0].Text;
            Assert.Equal("I00001", code);
            Assert.Equal("Successful.", text);

            // 2 is declined
            Assert.Equal("2", response.TransactionResponse.ResponseCode);

            Assert.Equal("2", response.TransactionResponse.Errors[0].ErrorCode);
            Assert.Equal("This transaction has been declined.", response.TransactionResponse.Errors[0].ErrorText);

            // failed transactions still have transaction ids
            Assert.False(string.IsNullOrEmpty(response.TransactionResponse.TransId));

            // auth code doesn't have values
            Assert.Equal(string.Empty, response.TransactionResponse.AuthCode);

            // https://support.authorize.net/s/article/What-Are-the-Different-Address-Verification-Service-AVS-Response-Codes
            Assert.Equal("Y", response.TransactionResponse.AvsResultCode);

            // https://support.authorize.net/s/article/Explaining-Card-Code-Mismatch-Declines
            Assert.Equal("N", response.TransactionResponse.CvvResultCode);

            // response can be ok but has errors
            Assert.Equal(MessageTypeEnum.Ok, response.Messages.ResultCode);
            Assert.True(response.TransactionResponse.Errors.Any());
        }

        [Fact]
        public async Task Credit_Card_Charge_For_ZipCode_46205_Test()
        {
            var services = new ServiceCollection();
            services.AddSingleton(_configuration);
            services.AddLogging(b => b.AddXunit(_output));
            services.AddAuthorizeNet();
            var sp = services.BuildServiceProvider();

            var client = sp.GetRequiredService<ITransactionClient>();
            var options = sp.GetRequiredService<IOptions<AuthorizeNetOptions>>();

            var carNumber = "5424000000000015";

            var request = new CreateTransactionRequest
            {
                TransactionRequest = new TransactionRequestType
                {
                    CustomerIP = options.Value.IpAddress,
                    TransactionType = Enum.GetName(typeof(TransactionTypeEnum), TransactionTypeEnum.AuthOnlyTransaction),
                    Amount = 1.0m,
                    Payment = new PaymentType
                    {
                        CreditCard = new CreditCardType
                        {
                            CardNumber = carNumber,
                            ExpirationDate = "2021-12",
                            CardCode = "901" // N   Does not match.
                        },
                    },
                    Customer = new CustomerDataType
                    {
                        Id = "cc-tester-56789",
                        Type = CustomerTypeEnum.Individual
                    },
                    Order = new OrderType
                    {
                        InvoiceNumber = "cc-invoice-123"
                    },
                    BillTo = new CustomerAddressType
                    {
                        Address = "1600 Pennsylvania Avenue NW",
                        City = "Washington",
                        State = "DC",
                        Zip = "46205" // causes FDS - Authorized/Pending Review
                    }
                },
            };

            var response = await client.CreateAsync(request);

            _output.WriteLine($"Account:{response.TransactionResponse.AccountNumber}" +
                $"TransId: [{response.TransactionResponse.TransId}]; " +
                $"CVV: [{response.TransactionResponse.CvvResultCode}]; " +
                $"AVS: [{response.TransactionResponse.AvsResultCode}]; " +
                $"CAVV: [{response.TransactionResponse.CavvResultCode}]");

            Assert.False(response.TransactionResponse.Errors.Any());
            Assert.Equal(MessageTypeEnum.Ok, response.Messages.ResultCode);

            var code = response.Messages.Message[0].Code;
            var text = response.Messages.Message[0].Text;
            Assert.Equal("I00001", code);
            Assert.Equal("Successful.", text);

            var accountNum = response.TransactionResponse.AccountNumber;

            Assert.Equal(carNumber.Substring(carNumber.Length - 4, 4), accountNum.Substring(accountNum.Length - 4, 4));

            // 4 is Authorized/Pending Review
            Assert.Equal("4", response.TransactionResponse.ResponseCode);

            Assert.Equal("253", response.TransactionResponse.Messages[0].Code);
            Assert.Equal("Your order has been received. Thank you for your business!", response.TransactionResponse.Messages[0].Description);

            Assert.False(string.IsNullOrEmpty(response.TransactionResponse.TransId));

            // auth code exist on the successful transactions
            Assert.True(!string.IsNullOrEmpty(response.TransactionResponse.AuthCode));

            // https://support.authorize.net/s/article/What-Are-the-Different-Address-Verification-Service-AVS-Response-Codes
            Assert.Equal("N", response.TransactionResponse.AvsResultCode);

            // https://support.authorize.net/s/article/Explaining-Card-Code-Mismatch-Declines
            Assert.Equal("N", response.TransactionResponse.CvvResultCode);
        }
    }
}
