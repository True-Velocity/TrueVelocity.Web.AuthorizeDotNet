using System;
using System.Threading;
using System.Threading.Tasks;

using AuthorizeNet.Api.V1.Contracts;

using Bet.Extensions.AuthorizeNet.Api.V1.Clients;

using Microsoft.Extensions.Logging;

namespace AuthorizeNet.Worker.Services
{
    public class TransactionService
    {
        private readonly ITransactionClient _transactionClient;
        private readonly ILogger<TransactionService> _logger;

        public TransactionService(
            ITransactionClient transactionClient,
            ILogger<TransactionService> logger)
        {
            _transactionClient = transactionClient ?? throw new ArgumentNullException(nameof(transactionClient));
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

            var request = new CreateTransactionRequest
            {
                RefId = refId,
                TransactionRequest = new TransactionRequestType
                {
                    TransactionType = Enum.GetName(typeof(TransactionTypeEnum), TransactionTypeEnum.AuthCaptureTransaction),
                    Amount = 5.0m,
                    Payment = new PaymentType
                    {
                        CreditCard = new CreditCardType
                        {
                            CardNumber = "5424000000000015",
                            ExpirationDate = "2021-12",
                            CardCode = "999"
                        }
                    }
                },
            };

            var response = await _transactionClient.CreateAsync(request, cancellationToken);

            _logger.LogInformation("Created AuthCapture Transaction: {code}", response.Messages.ResultCode.ToString());

            var batchRequest = new GetSettledBatchListRequest
            {
                FirstSettlementDate = DateTime.Now.Subtract(TimeSpan.FromDays(2)),
                LastSettlementDate = DateTime.Now,
            };

            var batchResponse = await _transactionClient.GetBatchListAsync(batchRequest, cancellationToken);
            _logger.LogInformation("Created batch transaction list: {code}", batchResponse.Messages.ResultCode.ToString());

            var refundRequest = new CreateTransactionRequest
            {
                RefId = refId,
                TransactionRequest = new TransactionRequestType
                {
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
                    }
                },
            };

            var refundReponse = await _transactionClient.CreateAsync(refundRequest, cancellationToken);
            _logger.LogInformation("Created Refund Transaction: {code}", refundReponse.Messages.ResultCode.ToString());
        }
    }
}
