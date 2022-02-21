using AuthorizeNet.Api.V1.Contracts;

namespace Bet.Extensions.AuthorizeNet.Api.V1.Clients;

public interface ICustomerShippingAddressClient
{
    Task<CreateCustomerShippingAddressResponse> CreateAsync(CreateCustomerShippingAddressRequest request, CancellationToken cancellationToken = default);

    Task<DeleteCustomerShippingAddressResponse> DeleteAsync(DeleteCustomerShippingAddressRequest request, CancellationToken cancellationToken = default);

    Task<GetCustomerShippingAddressResponse> GetAsync(GetCustomerShippingAddressRequest request, CancellationToken cancellationToken = default);

    Task<UpdateCustomerShippingAddressResponse> UpdateAsync(UpdateCustomerShippingAddressRequest request, CancellationToken cancellationToken = default);
}
