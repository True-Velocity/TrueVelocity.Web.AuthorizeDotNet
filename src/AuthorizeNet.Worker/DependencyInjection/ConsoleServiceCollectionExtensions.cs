using AuthorizeNet.Worker;
using AuthorizeNet.Worker.Services;

using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConsoleServiceCollectionExtensions
    {
        public static void ConfigureServices(HostBuilderContext hostBuilder, IServiceCollection services)
        {
            services.AddScoped<IMain, Main>();

            // add authorize registration
            services.AddAuthorizeNet();

            // add services
            services.AddScoped<CustomerService>();
            services.AddScoped<TransactionService>();
            services.AddScoped<SampleData>();
        }
    }
}
