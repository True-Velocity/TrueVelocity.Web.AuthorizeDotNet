using System.Threading;
using System.Threading.Tasks;

using AuthorizeNet.Api.V1.Contracts;

namespace Bet.Extensions.AuthorizeNet.Api.V1.Clients
{
    public class CustomerProfileClient : ICustomerProfileClient
    {
        private readonly IAuthorizeNetClient<CreateCustomerProfileRequest, CreateCustomerProfileResponse> _create;
        private readonly IAuthorizeNetClient<GetCustomerProfileRequest, GetCustomerProfileResponse> _get;
        private readonly IAuthorizeNetClient<GetCustomerProfileIdsRequest, GetCustomerProfileIdsResponse> _getAll;
        private readonly IAuthorizeNetClient<DeleteCustomerProfileRequest, DeleteCustomerProfileResponse> _delete;
        private readonly IAuthorizeNetClient<UpdateCustomerProfileRequest, UpdateCustomerProfileResponse> _update;
        private readonly IAuthorizeNetClient<CreateCustomerProfileFromTransactionRequest, CreateCustomerProfileResponse> _createFromTrans;

        public CustomerProfileClient(
            IAuthorizeNetClient<CreateCustomerProfileRequest, CreateCustomerProfileResponse> create,
            IAuthorizeNetClient<GetCustomerProfileRequest, GetCustomerProfileResponse> get,
            IAuthorizeNetClient<GetCustomerProfileIdsRequest, GetCustomerProfileIdsResponse> getAll,
            IAuthorizeNetClient<DeleteCustomerProfileRequest, DeleteCustomerProfileResponse> delete,
            IAuthorizeNetClient<UpdateCustomerProfileRequest, UpdateCustomerProfileResponse> update,
            IAuthorizeNetClient<CreateCustomerProfileFromTransactionRequest, CreateCustomerProfileResponse> createFromTrans)
        {
            _create = create ?? throw new System.ArgumentNullException(nameof(create));
            _get = get ?? throw new System.ArgumentNullException(nameof(get));
            _getAll = getAll ?? throw new System.ArgumentNullException(nameof(getAll));
            _delete = delete ?? throw new System.ArgumentNullException(nameof(delete));
            _update = update ?? throw new System.ArgumentNullException(nameof(update));
            _createFromTrans = createFromTrans ?? throw new System.ArgumentNullException(nameof(createFromTrans));
        }

        public Task<CreateCustomerProfileResponse> CreateAsync(
            CreateCustomerProfileRequest request,
            CancellationToken cancellationToken = default)
        {
            return _create.PostAsync(request, cancellationToken);
        }

        public Task<CreateCustomerProfileResponse> CreateFromTransactionAsync(
            CreateCustomerProfileFromTransactionRequest request,
            CancellationToken cancellationToken = default)
        {
            return _createFromTrans.PostAsync(request, cancellationToken);
        }

        public Task<GetCustomerProfileIdsResponse> GetAllAsync(
            GetCustomerProfileIdsRequest request,
            CancellationToken cancellationToken = default)
        {
            return _getAll.PostAsync(request, cancellationToken);
        }

        public Task<GetCustomerProfileResponse> GetAsync(
            GetCustomerProfileRequest request,
            CancellationToken cancellationToken = default)
        {
            return _get.PostAsync(request, cancellationToken);
        }

        public Task<DeleteCustomerProfileResponse> DeleteAsync(
            DeleteCustomerProfileRequest request,
            CancellationToken cancellationToken = default)
        {
            return _delete.PostAsync(request, cancellationToken);
        }

        public Task<UpdateCustomerProfileResponse> UpdateAsync(
                UpdateCustomerProfileRequest request,
                CancellationToken cancellationToken = default)
        {
            return _update.PostAsync(request, cancellationToken);
        }
    }
}
