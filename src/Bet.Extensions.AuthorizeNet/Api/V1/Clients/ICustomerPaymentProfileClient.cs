using System.Threading;
using System.Threading.Tasks;

using AuthorizeNet.Api.V1.Contracts;

namespace Bet.Extensions.AuthorizeNet.Api.V1.Clients
{
    public interface ICustomerPaymentProfileClient
    {
        Task<CreateCustomerPaymentProfileResponse> CreateAsync(CreateCustomerPaymentProfileRequest request, CancellationToken cancellationToken = default);

        Task<DeleteCustomerPaymentProfileResponse> DeleteAsync(DeleteCustomerPaymentProfileRequest request, CancellationToken cancellationToken = default);

        Task<GetCustomerPaymentProfileResponse> GetAsync(GetCustomerPaymentProfileRequest request, CancellationToken cancellationToken = default);

        Task<GetCustomerPaymentProfileListResponse> GetListAsync(GetCustomerPaymentProfileListRequest request, CancellationToken cancellationToken = default);

        Task<UpdateCustomerPaymentProfileResponse> UpdateAsync(UpdateCustomerPaymentProfileRequest request, CancellationToken cancellationToken = default);

        Task<ValidateCustomerPaymentProfileResponse> ValidateAsync(ValidateCustomerPaymentProfileRequest request, CancellationToken cancellationToken = default);
    }
}
