using System.Threading;
using System.Threading.Tasks;

using AuthorizeNet.Api.V1.Contracts;

namespace Bet.Extensions.AuthorizeNet.Api.V1.Clients
{
    public interface ICustomerProfileClient
    {
        Task<CreateCustomerProfileResponse> CreateAsync(CreateCustomerProfileRequest request, CancellationToken cancellationToken = default);

        Task<DeleteCustomerProfileResponse> DeleteAsync(DeleteCustomerProfileRequest request, CancellationToken cancellationToken = default);

        Task<GetCustomerProfileResponse> GetAsync(GetCustomerProfileRequest request, CancellationToken cancellationToken = default);
    }
}
