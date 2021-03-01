using System.Threading;
using System.Threading.Tasks;

using AuthorizeNet.Api.V1.Contracts;

namespace Bet.Extensions.AuthorizeNet.Api.V1.Clients
{
    public interface ICustomerProfileClient
    {
        /// <summary>
        /// Creates Authorize.Net Customer Profile.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<CreateCustomerProfileResponse> CreateAsync(CreateCustomerProfileRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes Authorize.Net Customer Profile.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DeleteCustomerProfileResponse> DeleteAsync(DeleteCustomerProfileRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves Authorize.Net Customer Profile.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<GetCustomerProfileResponse> GetAsync(GetCustomerProfileRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates Authorize.Net Customer Profile.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<UpdateCustomerProfileResponse> UpdateAsync(UpdateCustomerProfileRequest request, CancellationToken cancellationToken = default);
    }
}
