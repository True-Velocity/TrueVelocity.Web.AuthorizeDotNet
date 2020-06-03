using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
                var customerProfileClient = scope.ServiceProvider.GetRequiredService<CustomerProfileClient>();

                var result = await customerProfileClient.CreateCustomerAsync(stoppingToken);

                _logger.LogDebug(result.Messages.ResultCode.ToString());

                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(TimeSpan.FromSeconds(20), stoppingToken);
            }
        }
    }
}
