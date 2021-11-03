namespace Bet.Extensions.AuthorizeNet;

public sealed class Utf8StringWriter : StringWriter
{
    public override System.Text.Encoding Encoding => System.Text.Encoding.UTF8;
}
