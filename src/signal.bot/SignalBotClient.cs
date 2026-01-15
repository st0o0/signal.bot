using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Signal.Bot.Args;
using Signal.Bot.Internal;
using Signal.Bot.Requests;

namespace Signal.Bot;

public class SignalBotClient : ISignalBotClient
{
    private readonly SignalBotClientOptions _options;
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _serializerOptions;

    private readonly IHandlerRegistry<OnApiRequestArgs> _onApiRequestHandlers
        = new HandlerRegistry<OnApiRequestArgs>();

    private readonly IHandlerRegistry<OnApiResponseArgs> _onApiResponseHandlers
        = new HandlerRegistry<OnApiResponseArgs>();


    public SignalBotClient(string token, HttpClient httpClient, CancellationToken cancellationToken = default) : this(
        new SignalBotClientOptions(token), httpClient, cancellationToken)
    {
    }

    public SignalBotClient(SignalBotClientOptions options, HttpClient httpClient,
        CancellationToken cancellationToken = default)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _httpClient = httpClient;
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        GlobalCancelToken = cancellationToken;
    }


    public CancellationToken GlobalCancelToken { get; }

    public string Number => _options.Number;

    public TimeSpan Timeout => _httpClient.Timeout;

    public void AddOnMakingApiRequestHandler(Func<OnApiRequestArgs, CancellationToken, Task> handler)
        => _onApiRequestHandlers.Register(handler);

    public void AddOnApiResponseReceivedHandler(Func<OnApiResponseArgs, CancellationToken, Task> handler)
        => _onApiResponseHandlers.Register(handler);

    public async Task<TResponse> SendRequestAsync<TResponse>(IRequest<TResponse> request,
        string[]? queryParameters = null,
        CancellationToken cancellationToken = default)
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(GlobalCancelToken, cancellationToken);
        cancellationToken = cts.Token;
        var methodName = request.MethodName;
        var query = new List<string>(queryParameters ?? []);
        if (request is SearchNumbersRequest { Numbers: not null } searchRequest)
        {
            query.AddRange(searchRequest.Numbers.Select(num => $"numbers={Uri.EscapeDataString(num)}"));
        }

        if (query.Count > 0)
        {
            methodName += "?" + string.Join("&", query);
        }

        var httpRequest = new HttpRequestMessage(request.HttpMethod, methodName)
        {
            Content = request.ToHttpContent()
        };

        var response = await _httpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
        var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        return JsonSerializer.Deserialize<TResponse>(content, _serializerOptions)!;
    }

    public async Task SendRequestAsync(IRequest request, string[]? queryParameters = null,
        CancellationToken cancellationToken = default)
    {
        var methodName = request.MethodName;
        var query = new List<string>(queryParameters ?? []);
        if (request is SearchNumbersRequest { Numbers: not null } searchRequest)
        {
            query.AddRange(searchRequest.Numbers.Select(num => $"numbers={Uri.EscapeDataString(num)}"));
        }

        if (query.Count > 0)
        {
            methodName += "?" + string.Join("&", query);
        }

        var httpRequest = new HttpRequestMessage(request.HttpMethod, methodName)
        {
            Content = request.ToHttpContent()
        };

        var response = await _httpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
    }
}