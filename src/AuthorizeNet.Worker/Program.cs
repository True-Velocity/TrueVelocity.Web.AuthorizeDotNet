using AuthorizeNet.Worker.Services;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AuthorizeNet.Worker
{
    public sealed class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.AddHostedService<Worker>();
                        services.AddAuthorizeNet();
                        services.AddScoped<CustomerService>();
                        services.AddScoped<TransactionService>();
                    });
        }
    }
}
