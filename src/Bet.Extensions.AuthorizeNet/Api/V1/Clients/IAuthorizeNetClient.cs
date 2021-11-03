using AuthorizeNet.Api.V1.Contracts;

namespace Bet.Extensions.AuthorizeNet.Api.V1.Clients;

public interface IAuthorizeNetClient<TRequest, TResponse>
    where TRequest : ANetApiRequest
    where TResponse : ANetApiResponse
{
    /// <summary>
    /// Generic <see cref="HttpClient"/> post method implementation.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TResponse> PostAsync(TRequest request, CancellationToken cancellationToken = default);
}
