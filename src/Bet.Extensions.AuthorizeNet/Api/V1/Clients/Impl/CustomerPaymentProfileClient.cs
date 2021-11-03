using AuthorizeNet.Api.V1.Contracts;

namespace Bet.Extensions.AuthorizeNet.Api.V1.Clients.Impl;

public class CustomerPaymentProfileClient : ICustomerPaymentProfileClient
{
    private readonly IAuthorizeNetClient<CreateCustomerPaymentProfileRequest, CreateCustomerPaymentProfileResponse> _create;
    private readonly IAuthorizeNetClient<GetCustomerPaymentProfileRequest, GetCustomerPaymentProfileResponse> _get;
    private readonly IAuthorizeNetClient<GetCustomerPaymentProfileListRequest, GetCustomerPaymentProfileListResponse> _getList;
    private readonly IAuthorizeNetClient<UpdateCustomerPaymentProfileRequest, UpdateCustomerPaymentProfileResponse> _update;
    private readonly IAuthorizeNetClient<DeleteCustomerPaymentProfileRequest, DeleteCustomerPaymentProfileResponse> _delete;
    private readonly IAuthorizeNetClient<ValidateCustomerPaymentProfileRequest, ValidateCustomerPaymentProfileResponse> _validate;

    public CustomerPaymentProfileClient(
        IAuthorizeNetClient<CreateCustomerPaymentProfileRequest, CreateCustomerPaymentProfileResponse> create,
        IAuthorizeNetClient<GetCustomerPaymentProfileRequest, GetCustomerPaymentProfileResponse> get,
        IAuthorizeNetClient<GetCustomerPaymentProfileListRequest, GetCustomerPaymentProfileListResponse> getList,
        IAuthorizeNetClient<UpdateCustomerPaymentProfileRequest, UpdateCustomerPaymentProfileResponse> update,
        IAuthorizeNetClient<DeleteCustomerPaymentProfileRequest, DeleteCustomerPaymentProfileResponse> delete,
        IAuthorizeNetClient<ValidateCustomerPaymentProfileRequest, ValidateCustomerPaymentProfileResponse> validate)
    {
        _create = create;
        _get = get;
        _getList = getList;
        _update = update;
        _delete = delete;
        _validate = validate;
    }

    public Task<CreateCustomerPaymentProfileResponse> CreateAsync(
        CreateCustomerPaymentProfileRequest request,
        CancellationToken cancellationToken = default)
    {
        return _create.PostAsync(request, cancellationToken);
    }

    public Task<GetCustomerPaymentProfileResponse> GetAsync(
        GetCustomerPaymentProfileRequest request,
        CancellationToken cancellationToken = default)
    {
        return _get.PostAsync(request, cancellationToken);
    }

    public Task<GetCustomerPaymentProfileListResponse> GetListAsync(
        GetCustomerPaymentProfileListRequest request,
        CancellationToken cancellationToken = default)
    {
        return _getList.PostAsync(request, cancellationToken);
    }

    public Task<UpdateCustomerPaymentProfileResponse> UpdateAsync(
        UpdateCustomerPaymentProfileRequest request,
        CancellationToken cancellationToken = default)
    {
        return _update.PostAsync(request, cancellationToken);
    }

    public Task<DeleteCustomerPaymentProfileResponse> DeleteAsync(
        DeleteCustomerPaymentProfileRequest request,
        CancellationToken cancellationToken = default)
    {
        return _delete.PostAsync(request, cancellationToken);
    }

    public Task<ValidateCustomerPaymentProfileResponse> ValidateAsync(
        ValidateCustomerPaymentProfileRequest request,
        CancellationToken cancellationToken = default)
    {
        return _validate.PostAsync(request, cancellationToken);
    }
}
