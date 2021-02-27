using AuthorizeNet.Worker;
using AuthorizeNet.Worker.Options;
using AuthorizeNet.Worker.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConsoleServiceCollectionExtensions
    {
        public static void ConfigureServices(HostBuilderContext hostBuilder, IServiceCollection services)
        {
            services.AddScoped<IMain, Main>();

            services
                .AddOptions<AppOptions>()
                .Configure<IConfiguration>((options, config) =>
                {
                    config.Bind(nameof(AppOptions), options);
                });

            // add authorize registration
            services.AddAuthorizeNet();

            // add services
            services.AddScoped<CustomerService>();
            services.AddScoped<TransactionService>();
            services.AddScoped<SampleData>();
        }
    }
}
