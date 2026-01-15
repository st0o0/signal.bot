using Signal.Bot;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddHttpClient("signalbot_client")
    .AddTypedClient<ISignalBotClient>((httpClient, sp) => new SignalBotClient("", httpClient))
    .AddStandardResilienceHandler(options =>
    {
        // Retry
        options.Retry.MaxRetryAttempts = 3;
        options.Retry.Delay = TimeSpan.FromMilliseconds(200);

        // Circuit Breaker
        options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(30);
        options.CircuitBreaker.FailureRatio = 0.5;
        options.CircuitBreaker.MinimumThroughput = 10;

        // Timeout (per request)
        options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(10);
    });

var app = builder.Build();

await app.RunAsync();