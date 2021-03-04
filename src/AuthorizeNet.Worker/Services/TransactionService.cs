using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AuthorizeNet.Api.V1.Contracts;

using Bet.Extensions.AuthorizeNet.Api.V1.Clients;
using Bet.Extensions.AuthorizeNet.Options;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AuthorizeNet.Worker.Services
{
    public class TransactionService
    {
        private readonly ITransactionClient _transactionClient;
        private readonly AuthorizeNetOptions _options;
        private readonly ILogger<TransactionService> _logger;

        public TransactionService(
            ITransactionClient transactionClient,
            IOptions<AuthorizeNetOptions> options,
            ILogger<TransactionService> logger)
        {
            _transactionClient = transactionClient ?? throw new ArgumentNullException(nameof(transactionClient));
            _options = options.Value;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// https://developer.authorize.net/api/reference/index.html#payment-transactions.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task TestTransactionAsync(CancellationToken cancellationToken)
        {
            var refId = new Random(1000).Next().ToString();

            var createRequest = new CreateTransactionRequest
            {
                RefId = refId,
                TransactionRequest = new TransactionRequestType
                {
                    CustomerIP = _options.IpAddress,
                    TransactionType = Enum.GetName(typeof(TransactionTypeEnum), TransactionTypeEnum.AuthOnlyTransaction),
                    Amount = 1.0m,
                    Payment = new PaymentType
                    {
                        CreditCard = new CreditCardType
                        {
                            CardNumber = "5424000000000015",
                            ExpirationDate = "2021-12",
                            CardCode = "901"
                        },
                    },
                    Customer = new CustomerDataType
                    {
                        Id = "test-56789"
                    },
                    Order = new OrderType
                    {
                        InvoiceNumber = "invoice-123"
                    }
                },
            };

            var createResponse = await _transactionClient.CreateAsync(createRequest, cancellationToken);
            _logger.LogInformation("Created AuthCapture Transaction: {code}", createResponse.Messages.ResultCode.ToString());
            DisplayResponse("CreateTransaction", createResponse);

            // transaction is successful then proceed with cancellation.
            if (createResponse.Messages.ResultCode == MessageTypeEnum.Ok
                && createResponse.TransactionResponse?.Errors?.Count == 0)
            {
                var createTransId = createResponse.TransactionResponse.TransId;

                var voidRequest = new CreateTransactionRequest
                {
                    RefId = refId,
                    TransactionRequest = new TransactionRequestType
                    {
                        CustomerIP = _options.IpAddress,
                        TransactionType = Enum.GetName(typeof(TransactionTypeEnum), TransactionTypeEnum.VoidTransaction),
                        RefTransId = createTransId
                    },
                };

                var voidResponse = await _transactionClient.CreateAsync(voidRequest, cancellationToken);
                _logger.LogInformation(
                    "Voided AuthCapture Transaction: {code} - {transId}",
                    createResponse.Messages.ResultCode.ToString(),
                    voidResponse.TransactionResponse.TransId);
                DisplayResponse("VoidTransaction", voidResponse);
            }
            else
            {
                _logger.LogWarning("Code: {code} - {text}", createResponse.TransactionResponse.Errors[0].ErrorCode, createResponse.TransactionResponse.Errors[0].ErrorText);
            }
        }

        public async Task GetSettledBatch(CancellationToken cancellationToken)
        {
            var batchRequest = new GetSettledBatchListRequest
            {
                FirstSettlementDate = DateTime.Now.Subtract(TimeSpan.FromDays(2)),
                LastSettlementDate = DateTime.Now,
            };

            var batchResponse = await _transactionClient.GetBatchListAsync(batchRequest, cancellationToken);
            _logger.LogInformation("Created batch transaction list: {code}", batchResponse.Messages.ResultCode.ToString());
        }

        public async Task RefundTransaction(CancellationToken cancellationToken)
        {
            var refId = new Random(1000).Next().ToString();

            var refundRequest = new CreateTransactionRequest
            {
                RefId = refId,
                TransactionRequest = new TransactionRequestType
                {
                    CustomerIP = _options.IpAddress,
                    TransactionType = Enum.GetName(typeof(TransactionTypeEnum), TransactionTypeEnum.RefundTransaction),
                    Amount = 2.0m,
                    Payment = new PaymentType
                    {
                        CreditCard = new CreditCardType
                        {
                            CardNumber = "5424000000000015",
                            ExpirationDate = "2021-12",
                            CardCode = "999"
                        }
                    },
                },
            };

            var refundReponse = await _transactionClient.CreateAsync(refundRequest, cancellationToken);
            _logger.LogInformation(
                "Refund Transaction: {code} - {transId}",
                refundReponse.Messages.ResultCode.ToString(),
                refundReponse.TransactionResponse.TransId);
        }

        public async Task GetUnsettledTransactionAsync(CancellationToken cancellationToken)
        {
            var pageNumber = 1;
            var list = new List<GetUnsettledTransactionListResponse>();

            var result = await GetUnsettlePageAsync(pageNumber, list, cancellationToken);

            _logger.LogInformation("{count}", result.Count());
        }

        private void DisplayResponse(string action, ANetApiResponse response)
        {
            _logger.LogInformation("{action} - {code}", action, response?.Messages?.ResultCode.ToString());
            _logger.LogInformation("{action} - Code:{code}; Text:{text}", action, response?.Messages?.Message[0].Code, response?.Messages?.Message[0].Text);
        }

        private async Task<List<GetUnsettledTransactionListResponse>> GetUnsettlePageAsync(
            int pageNumber,
            List<GetUnsettledTransactionListResponse> refList,
            CancellationToken cancellationToken = default)
        {
            var request = new GetUnsettledTransactionListRequest
            {
                Paging = new Paging
                {
                    Limit = 10,
                    Offset = pageNumber
                },
                Status = TransactionGroupStatusEnum.Any,
            };

            var response = await _transactionClient.GetUnsettledListAsync(request, cancellationToken);
            if (response.Transactions.Count != 0)
            {
               refList.Add(response);
               await GetUnsettlePageAsync(pageNumber + 1, refList, cancellationToken);
            }

            return refList;
        }
    }
}
