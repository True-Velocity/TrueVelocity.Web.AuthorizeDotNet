namespace Bet.Extensions.AuthorizeNet;

internal class AuthorizeNetClientOptions : IAuthorizeNetClientOptions
{
    public AuthorizeNetClientOptions(string name)
    {
        Name = name;
    }

    public string Name { get;  }
}
