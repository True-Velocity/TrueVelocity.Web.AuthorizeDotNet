using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

using AuthorizeNet.Api.V1.Contracts;
using AuthorizeNet.Worker.Services;

using Bet.Extensions.AuthorizeNet.Api.V1.Clients;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AuthorizeNet.Worker
{
    public class Worker : BackgroundService
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<Worker> _logger;

        public Worker(IServiceProvider provider, ILogger<Worker> logger)
        {
            _provider = provider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _provider.CreateScope();
                var sp = scope.ServiceProvider;
                var customerService = sp.GetRequiredService<CustomerService>();
                var transactionService = sp.GetRequiredService<TransactionService>();

                // await customerService.TestCustomerProfileAsync(stoppingToken);

                await transactionService.TestTransactionAsync(stoppingToken);

                await Task.Delay(TimeSpan.FromSeconds(40), stoppingToken);
            }
        }
    }
}
