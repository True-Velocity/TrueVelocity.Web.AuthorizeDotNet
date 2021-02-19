using System;
using System.Threading;
using System.Threading.Tasks;

using AuthorizeNet.Worker.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AuthorizeNet.Worker
{
    public class Main : IMain
    {
        private readonly ILogger<Main> _logger;
        private readonly CustomerService _customerService;
        private readonly TransactionService _transactionService;
        private readonly IHostApplicationLifetime _applicationLifetime;

        public IConfiguration Configuration { get; set; }

        public Main(
            CustomerService customerService,
            TransactionService transactionService,
            IHostApplicationLifetime applicationLifetime,
            IConfiguration configuration,
            ILogger<Main> logger)
        {
            _customerService = customerService;
            _transactionService = transactionService;
            _applicationLifetime = applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime));
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<int> RunAsync()
        {
            // use this token for stopping the services
            _applicationLifetime.ApplicationStopping.ThrowIfCancellationRequested();

            var ts = CancellationTokenSource.CreateLinkedTokenSource(_applicationLifetime.ApplicationStopping);

            await _customerService.TestCustomerProfileAsync(ts.Token);

            await _transactionService.TestTransactionAsync(ts.Token);

            _logger.LogInformation("Main executed");

            return await Task.FromResult(0);
        }
    }
}
