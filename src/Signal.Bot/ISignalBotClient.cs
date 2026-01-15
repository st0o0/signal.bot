using System;
using System.Threading;
using System.Threading.Tasks;
using Signal.Bot.Args;
using Signal.Bot.Requests;

namespace Signal.Bot;

public interface ISignalBotClient
{
    /// <summary>Unique identifier for the bot from bot token, extracted from the first part of the bot token.
    /// Token format is not public API so this property is optional and may stop working in the future if Telegram changes it's token format.</summary>
    string Number { get; }

    /// <summary>Timeout for requests</summary>
    TimeSpan Timeout { get; }

    /// <summary>Occurs before sending a request to API</summary>
    void AddOnMakingApiRequestHandler(Func<OnApiRequestArgs, CancellationToken, Task> handler);

    /// <summary>Occurs after receiving the response to an API request</summary>
    void AddOnApiResponseReceivedHandler(Func<OnApiResponseArgs, CancellationToken, Task> handler);

    Task<TResponse> SendRequestAsync<TResponse>(
        IRequest<TResponse> request,
        string[]? queryParameters = null,
        CancellationToken cancellationToken = default
    );

    Task SendRequestAsync(
        IRequest request,
        string[]? queryParameters = null,
        CancellationToken cancellationToken = default);
}