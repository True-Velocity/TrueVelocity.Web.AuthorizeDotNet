using AuthorizeNet.Api.V1.Contracts;

namespace Bet.Extensions.AuthorizeNet.Api.V1.Clients;

public interface ICustomerProfileClient
{
    /// <summary>
    /// Creates Authorize.Net Customer Profile.
    /// Use this function to create a new customer profile including any customer payment profiles and customer shipping addresses.
    /// See <see href="https://developer.authorize.net/api/reference/index.html#customer-profiles-create-customer-profile"/>.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<CreateCustomerProfileResponse> CreateAsync(CreateCustomerProfileRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    ///  Create a Customer Profile from a Transaction.
    ///  This request enables you to create a customer profile, payment profile, and shipping profile from an existing successful transaction.
    ///  See <see href="https://developer.authorize.net/api/reference/index.html#customer-profiles-create-a-customer-profile-from-a-transaction"/>.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<CreateCustomerProfileResponse> CreateFromTransactionAsync(CreateCustomerProfileFromTransactionRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes Authorize.Net Customer Profile.
    /// Use this function to delete an existing customer profile along with all associated customer payment profiles and customer shipping addresses.
    /// See <see href="https://developer.authorize.net/api/reference/index.html#customer-profiles-delete-customer-profile"/>.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<DeleteCustomerProfileResponse> DeleteAsync(DeleteCustomerProfileRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves Authorize.Net Customer Profile.
    /// Use this function to retrieve an existing customer profile along with all the associated payment profiles and shipping addresses.
    /// See <see href="https://developer.authorize.net/api/reference/index.html#customer-profiles-get-customer-profile"/>.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<GetCustomerProfileResponse> GetAsync(GetCustomerProfileRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieved All Authorize.Net Customer Profiles Ids.
    /// Use this function to retrieve all existing customer profile IDs.
    /// See <see href="https://developer.authorize.net/api/reference/index.html#customer-profiles-get-customer-profile-ids"/>.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<GetCustomerProfileIdsResponse> GetAllAsync(GetCustomerProfileIdsRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates Authorize.Net Customer Profile.
    /// Use this function to update an existing customer profile.
    /// See <see href="https://developer.authorize.net/api/reference/index.html#customer-profiles-update-customer-profile"/>.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<UpdateCustomerProfileResponse> UpdateAsync(UpdateCustomerProfileRequest request, CancellationToken cancellationToken = default);
}
