using AuthorizeNet.Api.V1.Contracts;

namespace Bet.Extensions.AuthorizeNet.Api.V1.Clients;

/// <summary>
/// Authorize.Net Payment Transactions and Reporting.
/// </summary>
public interface ITransactionClient
{
    /// <summary>
    /// Create Transaction of the following types:
    /// - Charge a Credit Card
    /// - Authorize a Credit Card
    /// - Capture a Previously Authorized Amount
    /// - Refund a Transaction
    /// - Void a Transaction
    /// - Debit a Bank Account
    /// - Credit a Bank Account
    /// - Charge a Customer Profile
    /// - Charge a Tokenized Credit Card.
    /// See <see href="https://developer.authorize.net/api/reference/index.html#payment-transactions"/>.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<CreateTransactionResponse> CreateAsync(CreateTransactionRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get Transaction Details.
    /// Use this function to get detailed information about a specific transaction.
    /// See <see href="https://developer.authorize.net/api/reference/index.html#transaction-reporting-get-transaction-details"/>.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<GetTransactionDetailsResponse> GetAsync(GetTransactionDetailsRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get Settled Batch List.
    /// This function returns Batch ID, Settlement Time, and Settlement State for all settled batches with a range of dates.
    /// If includeStatistics is true, you also receive batch statistics by payment type and batch totals.
    /// All input parameters other than merchant authentication are optional.
    /// If no dates are specified, then the default is the past 24 hours, ending at the time of the call to getSettledBatchListRequest.
    /// See <see href="https://developer.authorize.net/api/reference/index.html#transaction-reporting-get-settled-batch-list"/>.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<GetSettledBatchListResponse> GetBatchListAsync(GetSettledBatchListRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get Transaction List.
    /// Use this function to return data for all transactions in a specified batch.
    /// The function will return data for up to 1000 of the most recent transactions in a single request.
    /// Paging options can be sent to limit the result set or to retrieve additional transactions beyond the 1000 transaction limit.
    /// No input parameters are required other than the authentication information and a batch ID.
    /// However, you can add the sorting and paging options shown below to customize the result set.
    /// See <see href="https://developer.authorize.net/api/reference/index.html#transaction-reporting-get-transaction-list"/>.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<GetTransactionListResponse> GetListAsync(GetTransactionListRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get Unsettled Transaction List.
    /// Use this function to get data for unsettled transactions.
    /// The function will return data for up to 1000 of the most recent transactions in a single request.
    /// Paging options can be sent to limit the result set or to retrieve additional transactions beyond the 1000 transaction limit.
    /// No input parameters are required other than the authentication information.
    /// However, you can add the sorting and paging options shown below to customize the result set.
    /// See <see href="https://developer.authorize.net/api/reference/index.html#transaction-reporting-get-unsettled-transaction-list"/>.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<GetUnsettledTransactionListResponse> GetUnsettledListAsync(GetUnsettledTransactionListRequest request, CancellationToken cancellationToken = default);
}
