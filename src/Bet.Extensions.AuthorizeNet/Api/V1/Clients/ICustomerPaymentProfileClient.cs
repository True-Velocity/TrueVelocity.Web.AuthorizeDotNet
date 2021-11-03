using AuthorizeNet.Api.V1.Contracts;

namespace Bet.Extensions.AuthorizeNet.Api.V1.Clients;

public interface ICustomerPaymentProfileClient
{
    /// <summary>
    /// Create Customer Payment Profile
    /// Use this function to create a new customer payment profile for an existing customer profile.
    /// See <see href="https://developer.authorize.net/api/reference/index.html#customer-profiles-create-customer-payment-profile"/>.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<CreateCustomerPaymentProfileResponse> CreateAsync(CreateCustomerPaymentProfileRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete Customer Payment Profile
    /// Use this function to delete a customer payment profile from an existing customer profile.
    /// See <see href="https://developer.authorize.net/api/reference/index.html#customer-profiles-delete-customer-payment-profile"/>.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<DeleteCustomerPaymentProfileResponse> DeleteAsync(DeleteCustomerPaymentProfileRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get Customer Payment Profile.
    /// Use this function to retrieve the details of a customer payment profile associated with an existing customer profile.
    /// Note: If the payment profile has previously been set as the default payment profile,
    /// you can submit this request using customerProfileId as the only parameter.
    /// Submitting this request with only the customer profile ID will cause the information for the default payment profile to be returned
    /// if a default payment profile has been previously designated.
    /// If no payment profile has been designated as the default payment profile, failing to specify a payment profile will result in an error.
    /// See <see href="https://developer.authorize.net/api/reference/index.html#customer-profiles-get-customer-payment-profile"/>.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<GetCustomerPaymentProfileResponse> GetAsync(GetCustomerPaymentProfileRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get Customer Payment Profile List.
    /// Use this function to get list of all the payment profiles that match the submitted searchType.
    /// You can use this function to get the list of all cards expiring this month.
    /// The function will return up to 10 results in a single request.
    /// Paging options can be sent to limit the result set or to retrieve additional results beyond the 10 item limit.
    /// You can add the sorting and paging options shown below to customize the result set.
    /// See <see href="https://developer.authorize.net/api/reference/index.html#customer-profiles-get-customer-payment-profile-list"/>.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<GetCustomerPaymentProfileListResponse> GetListAsync(GetCustomerPaymentProfileListRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update Customer Payment Profile.
    /// Use this function to update a payment profile for an existing customer profile.
    ///  Note: If some fields in this request are not submitted or are submitted with a blank value,
    ///  the values in the original profile are removed.As a best practice to prevent this from happening,
    ///  call getCustomerPaymentProfileRequest to receive all current information including masked payment information.
    ///  Change the field or fields that you wish to update, and then reuse all the fields you received, with updates,
    ///  in a call to updateCustomerPaymentProfileRequest.
    /// See <see href="https://developer.authorize.net/api/reference/index.html#customer-profiles-update-customer-payment-profile"/>.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<UpdateCustomerPaymentProfileResponse> UpdateAsync(UpdateCustomerPaymentProfileRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validate Customer Payment Profile.
    /// Use this function to generate a test transaction that verifies an existing customer payment profile.
    /// No customer receipt emails are sent when the validateCustomerPaymentProfileRequest function is called.
    /// See <see href="https://developer.authorize.net/api/reference/index.html#customer-profiles-validate-customer-payment-profile"/>.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ValidateCustomerPaymentProfileResponse> ValidateAsync(ValidateCustomerPaymentProfileRequest request, CancellationToken cancellationToken = default);
}
