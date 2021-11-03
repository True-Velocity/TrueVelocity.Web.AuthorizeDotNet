using AuthorizeNet.Worker.Services;

namespace AuthorizeNet.Worker;

public class Main : IMain
{
    private readonly ILogger<Main> _logger;
    private readonly CustomerService _customerService;
    private readonly TransactionService _transactionService;
    private readonly SampleData _sampleData;
    private readonly IHostApplicationLifetime _applicationLifetime;

    public Main(
        CustomerService customerService,
        TransactionService transactionService,
        SampleData sampleData,
        IHostApplicationLifetime applicationLifetime,
        IConfiguration configuration,
        ILogger<Main> logger)
    {
        _customerService = customerService;
        _transactionService = transactionService;
        _sampleData = sampleData ?? throw new ArgumentNullException(nameof(sampleData));
        _applicationLifetime = applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime));
        Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public IConfiguration Configuration { get; set; }

    public async Task<int> RunAsync()
    {
        // use this token for stopping the services
        _applicationLifetime.ApplicationStopping.ThrowIfCancellationRequested();

        var ts = CancellationTokenSource.CreateLinkedTokenSource(_applicationLifetime.ApplicationStopping);

        // create and deletes customer payment profiles.
        // foreach (var card in _sampleData.GetCustomerProfiles())
        // {
        //    await _customerService.TestCustomerProfileAsync(card, ts.Token);
        // }
        // await _transactionService.TestTransactionAsync(ts.Token);

        await _transactionService.GetUnsettledTransactionAsync(ts.Token);
        _logger.LogInformation("Main executed");

        return await Task.FromResult(0);
    }
}
