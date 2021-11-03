using AuthorizeNet.Api.V1.Contracts;

namespace AuthorizeNet.Worker.Models;

public class CustomerProfile
{
    public string? CustomerId { get; set; }

    public CustomerTypeEnum CustomerType { get; set; }

    public string? CardNumber { get; set; }

    public string? ExpirationDate { get; set; }

    public string? CardCode { get; set; }

    public CustomerProfileTypeEnum CustomerProfileType { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Description { get; set; }

    public string? StreetLine { get; set; }

    public string? Company { get; set; }

    public string? City { get; set; }

    public string? StateOrProvice { get; set; }

    public string? ZipCode { get; set; }

    public string? Country { get; set; }

    public string? ReferenceId { get; set; }
}
