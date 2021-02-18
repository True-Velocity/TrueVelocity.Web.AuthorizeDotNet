using System.Threading;
using System.Threading.Tasks;

using AuthorizeNet.Api.V1.Contracts;

namespace Bet.Extensions.AuthorizeNet.Api.V1.Clients
{
    public interface ITransactionClient
    {
        Task<CreateTransactionResponse> CreateAsync(CreateTransactionRequest request, CancellationToken cancellationToken = default);

        Task<GetTransactionDetailsResponse> GetAsync(GetTransactionDetailsRequest request, CancellationToken cancellationToken = default);

        Task<GetSettledBatchListResponse> GetBatchListAsync(GetSettledBatchListRequest request, CancellationToken cancellationToken = default);

        Task<GetTransactionListResponse> GetListAsync(GetTransactionListRequest request, CancellationToken cancellationToken = default);
    }
}