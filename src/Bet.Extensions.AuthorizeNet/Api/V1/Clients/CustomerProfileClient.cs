using System.Threading;
using System.Threading.Tasks;

using AuthorizeNet.Api.V1.Contracts;

namespace Bet.Extensions.AuthorizeNet.Api.V1.Clients
{
    public class CustomerProfileClient
    {
        private readonly IAuthorizeNetClient<CreateCustomerProfileRequest, CreateCustomerProfileResponse> _createCustomerProfile;
        private readonly IAuthorizeNetClient<GetCustomerProfileRequest, GetCustomerProfileResponse> _getCustomerProfile;
        private readonly IAuthorizeNetClient<DeleteCustomerProfileRequest, DeleteCustomerProfileResponse> _deletCustomerProfile;

        public CustomerProfileClient(
            IAuthorizeNetClient<CreateCustomerProfileRequest, CreateCustomerProfileResponse> createCustomerProfile,
            IAuthorizeNetClient<GetCustomerProfileRequest, GetCustomerProfileResponse> getCustomerProfile,
            IAuthorizeNetClient<DeleteCustomerProfileRequest, DeleteCustomerProfileResponse> deletCustomerProfile)
        {
            _createCustomerProfile = createCustomerProfile ?? throw new System.ArgumentNullException(nameof(createCustomerProfile));
            _getCustomerProfile = getCustomerProfile ?? throw new System.ArgumentNullException(nameof(getCustomerProfile));
            _deletCustomerProfile = deletCustomerProfile ?? throw new System.ArgumentNullException(nameof(deletCustomerProfile));
        }

        public async Task<CreateCustomerProfileResponse> CreateAsync(
            CreateCustomerProfileRequest request,
            CancellationToken cancellationToken = default)
        {
            return await _createCustomerProfile.PostAsync(request, cancellationToken);
        }

        public async Task<GetCustomerProfileResponse> GetAsync(
            GetCustomerProfileRequest request,
            CancellationToken cancellationToken = default)
        {
            return await _getCustomerProfile.PostAsync(request, cancellationToken);
        }

        public async Task<DeleteCustomerProfileResponse> DeleteAsync(
            DeleteCustomerProfileRequest request,
            CancellationToken cancellationToken = default)
        {
            return await _deletCustomerProfile.PostAsync(request, cancellationToken);
        }
    }
}
