# Bet.Extensions.AuthorizeNet

[![Build status](https://ci.appveyor.com/api/projects/status/62hg47fx8erd9rw4/branch/master?svg=true)](https://ci.appveyor.com/project/kdcllc/bet-extensions-authorizenet/branch/master)
[![NuGet](https://img.shields.io/nuget/v/Bet.Extensions.AuthorizeNet.svg)](https://www.nuget.org/packages?q=Bet.Extensions.AuthorizeNet)
![Nuget](https://img.shields.io/nuget/dt/Bet.Extensions.AuthorizeNet)
[![feedz.io](https://img.shields.io/badge/endpoint.svg?url=https://f.feedz.io/kdcllc/kdcllc/shield/Bet.Extensions.AuthorizeNet/latest)](https://f.feedz.io/kdcllc/kdcllc/packages/Bet.Extensions.AuthorizeNet/latest/download)

The goal of this repository is to implement Authorize.Net Web Api with the latest Microsoft DotNetCore technology.

[API Reference](https://developer.authorize.net/api/reference/index.html#apireferenceheader)

- Sandbox API Endpoint: `https://apitest.authorize.net/xml/v1/request.api`

- Production API Endpoint: `https://api.authorize.net/xml/v1/request.api`

## [Customer Profiles](https://developer.authorize.net/api/reference/features/customer_profiles.html)

- [ ] Create Customer Profile
- [ ] Get Customer Profile
- [ ] Get Customer Profile IDs
- [ ] Update Customer Profile
- [ ]
- [ ]
- [ ]
- [ ]
- [ ]
- [ ]
- [ ]
- [ ]

## Other functionality

- [ ] Support for `HttpClient` proxy configuration

## Regenerate Contracts

[XmlSchemaClassGenerator](https://github.com/mganss/XmlSchemaClassGenerator)

```dotnetcli

   dotnet xscgen -f AnetApiSchema.xsd -o Api\V1\Contracts -n =AuthorizeNet.Api.V1.Contracts --csm Public

   dotnet xscgen AnetApiSchema.xsd -o Api\V1\Contracts -n =AuthorizeNet.Api.V1.Contracts --csm Public -0

```