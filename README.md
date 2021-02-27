# Bet.Extensions.AuthorizeNet

[![Build status](https://ci.appveyor.com/api/projects/status/62hg47fx8erd9rw4/branch/master?svg=true)](https://ci.appveyor.com/project/kdcllc/bet-extensions-authorizenet/branch/master)
[![NuGet](https://img.shields.io/nuget/v/Bet.Extensions.AuthorizeNet.svg)](https://www.nuget.org/packages?q=Bet.Extensions.AuthorizeNet)
![Nuget](https://img.shields.io/nuget/dt/Bet.Extensions.AuthorizeNet)
[![feedz.io](https://img.shields.io/badge/endpoint.svg?url=https://f.feedz.io/kdcllc/kdcllc/shield/Bet.Extensions.AuthorizeNet/latest)](https://f.feedz.io/kdcllc/kdcllc/packages/Bet.Extensions.AuthorizeNet/latest/download)

The goal of this repository is to implement [Authorize.Net API](https://developer.authorize.net/api/upgrade_guide.html#aim) the latest Microsoft DotNetCore technology.

The implementation is build with `Microsoft.Extensions.Http.Polly` library for `HttpClient` management.

The solution is split into two projets:

1. `Bet.Extensions.AuthorizeNet` - base library that supports `dotnetcore` and `json`.
2. `AuthorizeNet.Worker` example was created using `[Bet.Extensions.Templating](https://github.com/kdcllc/Bet.Extensions.Templating)

[API Reference](https://developer.authorize.net/api/reference/index.html#apireferenceheader)

- Sandbox API Endpoint: `https://apitest.authorize.net/xml/v1/request.api`

- Production API Endpoint: `https://api.authorize.net/xml/v1/request.api`

## Authorize.Net Reference

### [Customer Profiles](https://developer.authorize.net/api/reference/features/customer_profiles.html)

https://github.com/AuthorizeNet/sample-code-csharp/tree/master/CustomerProfiles
`CustomerProfileClient` implements:

- [x] Create Customer Profile
- [x] Get Customer Profile
- [x] Get Customer Profile IDs
- [x] Update Customer Profile

### Customer Payment Profile

`CustomerPaymentProfileClient` implements:

- [x] Create Customer Payment Profile
- [x] Get Customer Payment Profile
- [x] Get Customer Payment Profile List
- [x] Update Customer Payment Profile
- [x] Delete Customer Payment Profile
- [x] Validate Customer Payment Profile

### [Transactions](https://github.com/AuthorizeNet/sample-code-csharp/tree/master/PaymentTransactions)

`TransactionClient` implements:

- [x] Create Transaction
- [x] Get Transaction Details
- [x] Get Settled Batch List
- [x] Get Transaction List

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
