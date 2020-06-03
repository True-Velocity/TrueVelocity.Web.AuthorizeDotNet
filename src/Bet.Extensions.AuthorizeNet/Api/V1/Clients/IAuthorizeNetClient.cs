using System.Threading;
using System.Threading.Tasks;

using AuthorizeNet.Api.V1.Contracts;

namespace Bet.Extensions.AuthorizeNet.Api.V1.Clients
{
    public interface IAuthorizeNetClient<TRequest, TResponse>
        where TRequest : ANetApiRequest
        where TResponse : ANetApiResponse
    {
        Task<TResponse> PostAsync(TRequest request, CancellationToken cancellationToken = default);
    }
}