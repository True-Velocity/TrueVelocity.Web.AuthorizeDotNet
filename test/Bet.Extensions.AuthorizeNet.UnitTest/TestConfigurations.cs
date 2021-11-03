using Microsoft.Extensions.Configuration;

namespace Bet.Extensions.AuthorizeNet.UnitTest;

public sealed class TestConfigurations
{
    public static IConfiguration GetConfiguration()
    {
        var inMemoryConfiguration = new Dictionary<string, string>
            {
                { "Test:SomeNode", "Hello World" },
            };

        var configBuilder = new ConfigurationBuilder();
        configBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        configBuilder.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
        configBuilder.AddInMemoryCollection(inMemoryConfiguration);

        return configBuilder.Build();
    }
}
