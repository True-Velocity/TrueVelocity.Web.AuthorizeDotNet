# Bet.Extensions.AuthorizeNet

[![Build status](https://ci.appveyor.com/api/projects/status/62hg47fx8erd9rw4/branch/master?svg=true)](https://ci.appveyor.com/project/kdcllc/bet-extensions-authorizenet/branch/master)
[![NuGet](https://img.shields.io/nuget/v/Bet.Extensions.AuthorizeNet.svg)](https://www.nuget.org/packages?q=Bet.Extensions.AuthorizeNet)
![Nuget](https://img.shields.io/nuget/dt/Bet.Extensions.AuthorizeNet)
[![feedz.io](https://img.shields.io/badge/endpoint.svg?url=https://f.feedz.io/kdcllc/kdcllc/shield/Bet.Extensions.AuthorizeNet/latest)](https://f.feedz.io/kdcllc/kdcllc/packages/Bet.Extensions.AuthorizeNet/latest/download)

The goal of this repository is to implement [Authorize.Net API](https://developer.authorize.net/api/upgrade_guide.html#aim) the latest Microsoft DotNetCore technology.

The implementation is build on top of `Microsoft.Extensions.Http.Polly` library for `HttpClient` management. It provides with an ability to provide custom `Polly` Policies if needed for resilience.

The solution is split into two projects:

1. `Bet.Extensions.AuthorizeNet` - base library that supports `dotnetcore` and `json`.
2. `AuthorizeNet.Worker` example was created using `[Bet.Extensions.Templating](https://github.com/kdcllc/Bet.Extensions.Templating)

[API Reference](https://developer.authorize.net/api/reference/index.html#apireferenceheader)

- Sandbox API Endpoint: `https://apitest.authorize.net/xml/v1/request.api`

- Production API Endpoint: `https://api.authorize.net/xml/v1/request.api`

## Authorize.Net Reference


### Customer Profile APIs - [`ICustomerProfileClient`](src\Bet.Extensions.AuthorizeNet\Api\V1\Clients\ICustomerProfileClient.cs)

- [x] [Create Customer Profile](https://developer.authorize.net/api/reference/index.html#customer-profiles)
- [x] [Create a Customer Profile from a Transaction](https://developer.authorize.net/api/reference/index.html#customer-profiles-create-a-customer-profile-from-a-transaction)
- [x] [Get Customer Profile](https://developer.authorize.net/api/reference/index.html#customer-profiles-get-customer-profile)
- [x] [Get Customer Profile IDs](https://developer.authorize.net/api/reference/index.html#customer-profiles-get-customer-profile-ids)
- [x] [Update Customer Profile](https://developer.authorize.net/api/reference/index.html#customer-profiles-update-customer-profile)
- [ ] [Create Customer Shipping Address](https://developer.authorize.net/api/reference/index.html#customer-profiles-create-customer-shipping-address)
- [ ] [Get Customer Shipping Address](https://developer.authorize.net/api/reference/index.html#customer-profiles-get-customer-shipping-address)
- [ ] [Update Customer Shipping Address](https://developer.authorize.net/api/reference/index.html#customer-profiles-update-customer-shipping-address)
- [ ] [Delete Customer Shipping Address](https://developer.authorize.net/api/reference/index.html#customer-profiles-delete-customer-shipping-address)

### Customer Payment Profile APIs - [`ICustomerPaymentProfileClient`](src\Bet.Extensions.AuthorizeNet\Api\V1\Clients\ICustomerProfileClient.cs)

- [x] [Create Customer Payment Profile](https://developer.authorize.net/api/reference/index.html#customer-profiles-create-customer-payment-profile)
- [x] [Get Customer Payment Profile](https://developer.authorize.net/api/reference/index.html#customer-profiles-get-customer-payment-profile)
- [x] [Get Customer Payment Profile List](https://developer.authorize.net/api/reference/index.html#customer-profiles-get-customer-payment-profile-list)
- [x] [Update Customer Payment Profile](https://developer.authorize.net/api/reference/index.html#customer-profiles-update-customer-payment-profile)
- [x] [Delete Customer Payment Profile](https://developer.authorize.net/api/reference/index.html#customer-profiles-delete-customer-payment-profile)
- [x] [Validate Customer Payment Profile](https://developer.authorize.net/api/reference/index.html#customer-profiles-validate-customer-payment-profile)

### Payment Transactions && Transaction Reporting APIs - [`TransactionClient`]()

- [x] Create Transaction 
  * [Charge a Credit Card](https://developer.authorize.net/api/reference/index.html#payment-transactions-charge-a-credit-card)
  * [Authorize a Credit Card](https://developer.authorize.net/api/reference/index.html#payment-transactions-authorize-a-credit-card)
  * [Capture a Previously Authorized Amount](https://developer.authorize.net/api/reference/index.html#payment-transactions-capture-a-previously-authorized-amount)
  * [Refund a Transaction](https://developer.authorize.net/api/reference/index.html#payment-transactions-refund-a-transaction)
  * [Void a Transaction](https://developer.authorize.net/api/reference/index.html#payment-transactions-void-a-transaction)
  * [Debit a Bank Account](https://developer.authorize.net/api/reference/index.html#payment-transactions-debit-a-bank-account)
  * [Credit a Bank Account](https://developer.authorize.net/api/reference/index.html#payment-transactions-credit-a-bank-account)
  * [Charge a Customer Profile](https://developer.authorize.net/api/reference/index.html#payment-transactions-charge-a-customer-profile)
  * [Charge a Tokenized Credit Card](https://developer.authorize.net/api/reference/index.html#payment-transactions-charge-a-tokenized-credit-card)
- [x] [Get Transaction Details](https://developer.authorize.net/api/reference/index.html#transaction-reporting-get-transaction-details)
- [x] [Get Settled Batch List](https://developer.authorize.net/api/reference/index.html#transaction-reporting-get-settled-batch-list)
- [x] [Get Transaction List](https://developer.authorize.net/api/reference/index.html#transaction-reporting-get-transaction-list)
- [x] [Get Unsettled Transaction List](https://developer.authorize.net/api/reference/index.html#transaction-reporting-get-unsettled-transaction-list)

## Authorize.net

- https://sandbox.authorize.net/
- https://login.authorize.net/

## Other functionality

- [x] Support for `HttpClient` proxy configuration

## Regenerated Contracts

[XmlSchemaClassGenerator](https://github.com/mganss/XmlSchemaClassGenerator)

```dotnetcli

   dotnet xscgen -f AnetApiSchema.xsd -o Api\V1\Contracts -n =AuthorizeNet.Api.V1.Contracts --csm Public

   dotnet xscgen AnetApiSchema.xsd -o Api\V1\Contracts -n =AuthorizeNet.Api.V1.Contracts --csm Public -0

```

### Sandbox testing

In order for profile create to work, we need to have TestMode set in the configuration.

```json

  "AuthorizeNetOptions": {
    "IsSandBox": true,
    "ClientName": "",
    "TransactionKey": "",
    "IpAddress": "",
    "ValidationMode": "TestMode"
  }
```

### eCheck.Net

- `accountType` - The type of bank account. Certain bank account types require you to use certain ACH transaction types. String, either checking, savings, or businessChecking. See the Understanding ACH Codes section below for details.
- `routingNumber` - The bank's routing number. String, up to 9 characters. For refunds, use "XXXX" plus the first four digits of the account number.
- `accountNumber` - The customer's account number. String, up to 17 characters. For refunds, use "XXXX" plus the first four digits of the account number.
- `nameOnAccount` - The name of the person who holds the bank account. String, up to 22 characters.
- `bankName` - The name of the bank String, up to 50 characters.
- `echeckType` - `web` the customer uses a website or smartphone app to authorize the transaction.

#### Testing in Sandbox

Test Routing Number:

- 021000021
- 011401533
- 09100001

Test Account Number:

- 111111111

Test Name on Account:

- Demo

For testing purposes, eCheck.Net transactions under $100 will be accepted. To generate a decline, submit a transactions over $100.

## Reference

- https://www.authorize.net/content/dam/anet-redesign/documents/AIM_guide.pdf
- [Upgrade Guide](https://developer.authorize.net/api/upgrade_guide.html)
- [Payment Transactions](https://developer.authorize.net/api/reference/features/payment_transactions.html)
- [Customer Profiles](https://developer.authorize.net/api/reference/features/customer_profiles.html)
- [eCheck.Net](https://developer.authorize.net/api/reference/features/echeck.html)
- [Authorize.net Test Credit Card Numbers](https://www.leadcommerce.com/support-articles/authorize.net-test-credit-card-numbers.html)
- [Validate Credit Card Numbers](https://www.oreilly.com/library/view/regular-expressions-cookbook/9781449327453/ch04s20.html)
- [Testing Guide](https://developer.authorize.net/hello_world/testing_guide.html)
- [responseCodes](https://developer.authorize.net/api/reference/responseCodes.html)
- [What Are the Different Address Verification Service AVS Response Codes](https://support.authorize.net/s/article/What-Are-the-Different-Address-Verification-Service-AVS-Response-Codes)
- [Go Live](https://developer.authorize.net/hello_world/go-live.html)
- [errorandresponsecodes](https://developer.authorize.net/api/reference/features/errorandresponsecodes.html)
- [eCheck.Net](https://developer.authorize.net/api/reference/features/echeck.html)
- [CustomerProfiles](https://github.com/AuthorizeNet/sample-code-csharp/tree/master/CustomerProfiles)
- [Transactions](https://github.com/AuthorizeNet/sample-code-csharp/tree/master/PaymentTransactions)
