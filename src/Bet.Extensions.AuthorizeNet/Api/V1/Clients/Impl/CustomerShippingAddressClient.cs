using AuthorizeNet.Api.V1.Contracts;

namespace Bet.Extensions.AuthorizeNet.Api.V1.Clients.Impl;

public class CustomerShippingAddressClient : ICustomerShippingAddressClient
{
    private readonly IAuthorizeNetClient<CreateCustomerShippingAddressRequest, CreateCustomerShippingAddressResponse> _create;
    private readonly IAuthorizeNetClient<GetCustomerShippingAddressRequest, GetCustomerShippingAddressResponse> _get;
    private readonly IAuthorizeNetClient<UpdateCustomerShippingAddressRequest, UpdateCustomerShippingAddressResponse> _update;
    private readonly IAuthorizeNetClient<DeleteCustomerShippingAddressRequest, DeleteCustomerShippingAddressResponse> _delete;

    public CustomerShippingAddressClient(
        IAuthorizeNetClient<CreateCustomerShippingAddressRequest, CreateCustomerShippingAddressResponse> create,
        IAuthorizeNetClient<GetCustomerShippingAddressRequest, GetCustomerShippingAddressResponse> get,
        IAuthorizeNetClient<UpdateCustomerShippingAddressRequest, UpdateCustomerShippingAddressResponse> update,
        IAuthorizeNetClient<DeleteCustomerShippingAddressRequest, DeleteCustomerShippingAddressResponse> delete)
    {
        _create = create;
        _get = get;
        _update = update;
        _delete = delete;
    }

    public Task<CreateCustomerShippingAddressResponse> CreateAsync(
        CreateCustomerShippingAddressRequest request,
        CancellationToken cancellationToken = default)
    {
        return _create.PostAsync(request, cancellationToken);
    }

    public Task<GetCustomerShippingAddressResponse> GetAsync(
        GetCustomerShippingAddressRequest request,
        CancellationToken cancellationToken = default)
    {
        return _get.PostAsync(request, cancellationToken);
    }

    public Task<UpdateCustomerShippingAddressResponse> UpdateAsync(
        UpdateCustomerShippingAddressRequest request,
        CancellationToken cancellationToken = default)
    {
        return _update.PostAsync(request, cancellationToken);
    }

    public Task<DeleteCustomerShippingAddressResponse> DeleteAsync(
        DeleteCustomerShippingAddressRequest request,
        CancellationToken cancellationToken = default)
    {
        return _delete.PostAsync(request, cancellationToken);
    }
}
