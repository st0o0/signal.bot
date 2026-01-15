using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Signal.Bot.Polling;
using Signal.Bot.Types;

namespace Signal.Bot;

public static partial class SignalBotClientExtensions
{
    public static void StartReceiving<TUpdateHandler>(this ISignalBotClient botClient,
        ReceiverOptions? receiverOptions = null,
        CancellationToken cancellationToken = default) where TUpdateHandler : IUpdateHandler, new()
        => botClient.StartReceiving(new TUpdateHandler(), receiverOptions, cancellationToken);

    public static void StartReceiving(this ISignalBotClient botClient,
        Func<ISignalBotClient, ReceivedMessage, CancellationToken, Task> updateHandler,
        Func<ISignalBotClient, Exception, HandleErrorSource, CancellationToken, Task> errorHandler,
        ReceiverOptions? receiverOptions = null, CancellationToken cancellationToken = default)
        => botClient.StartReceiving(new DefaultUpdateHandler(updateHandler, errorHandler), receiverOptions,
            cancellationToken);

    public static void StartReceiving(this ISignalBotClient botClient,
        Func<ISignalBotClient, ReceivedMessage, CancellationToken, Task> updateHandler,
        Func<ISignalBotClient, Exception, CancellationToken, Task> errorHandler,
        ReceiverOptions? receiverOptions = null, CancellationToken cancellationToken = default)
        => botClient.StartReceiving(new DefaultUpdateHandler(updateHandler, errorHandler), receiverOptions,
            cancellationToken);

    public static void StartReceiving(this ISignalBotClient botClient,
        Action<ISignalBotClient, ReceivedMessage, CancellationToken> updateHandler,
        Action<ISignalBotClient, Exception, CancellationToken> errorHandler,
        ReceiverOptions? receiverOptions = null, CancellationToken cancellationToken = default)
        => botClient.StartReceiving(new DefaultUpdateHandler(
            (bot, update, token) =>
            {
                updateHandler(bot, update, token);
                return Task.CompletedTask;
            },
            (bot, exception, source, token) =>
            {
                errorHandler(bot, exception, token);
                return Task.CompletedTask;
            }
        ), receiverOptions, cancellationToken);

    public static void StartReceiving(this ISignalBotClient botClient, IUpdateHandler updateHandler,
        ReceiverOptions? receiverOptions = null,
        CancellationToken cancellationToken = default)
    {
        if (botClient is null)
        {
            throw new ArgumentNullException(nameof(botClient));
        }

        if (updateHandler is null)
        {
            throw new ArgumentNullException(nameof(updateHandler));
        }

        // ReSharper disable once MethodSupportsCancellation
        _ = Task.Run(async () =>
        {
            try
            {
                await botClient.ReceiveAsync(updateHandler, receiverOptions, cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                // ignored
            }
            catch (Exception ex)
            {
                try
                {
                    await updateHandler.HandleErrorAsync(botClient, ex, HandleErrorSource.FatalError, cancellationToken)
                        .ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    // ignored
                }
            }
        }, cancellationToken);
    }

    public static async Task ReceiveAsync<TUpdateHandler>(this ISignalBotClient botClient,
        ReceiverOptions? receiverOptions = null,
        CancellationToken cancellationToken = default) where TUpdateHandler : IUpdateHandler, new()
        => await botClient.ReceiveAsync(new TUpdateHandler(), receiverOptions, cancellationToken)
            .ConfigureAwait(false);

    public static async Task ReceiveAsync(this ISignalBotClient botClient,
        Func<ISignalBotClient, ReceivedMessage, CancellationToken, Task> updateHandler,
        Func<ISignalBotClient, Exception, CancellationToken, Task> errorHandler,
        ReceiverOptions? receiverOptions = null, CancellationToken cancellationToken = default)
        => await botClient.ReceiveAsync(new DefaultUpdateHandler(updateHandler, errorHandler), receiverOptions,
            cancellationToken).ConfigureAwait(false);

    public static async Task ReceiveAsync(this ISignalBotClient botClient,
        Action<ISignalBotClient, ReceivedMessage, CancellationToken> updateHandler,
        Action<ISignalBotClient, Exception, CancellationToken> errorHandler,
        ReceiverOptions? receiverOptions = null, CancellationToken cancellationToken = default)
        => await botClient.ReceiveAsync(new DefaultUpdateHandler(
            (bot, update, token) =>
            {
                updateHandler(bot, update, token);
                return Task.CompletedTask;
            },
            (bot, exception, source, token) =>
            {
                errorHandler(bot, exception, token);
                return Task.CompletedTask;
            }
        ), receiverOptions, cancellationToken).ConfigureAwait(false);

    public static async Task ReceiveAsync(this ISignalBotClient botClient, IUpdateHandler updateHandler,
        ReceiverOptions? receiverOptions = null,
        CancellationToken cancellationToken = default)
    {
        var number = "";
        var uri = new Uri($"ws://127.0.0.1:8080/v1/receive/{number}");

        using var socket = new ClientWebSocket();

        await socket.ConnectAsync(uri, cancellationToken);

        var buffer = new byte[4 * 1024];
        while (socket.State == WebSocketState.Open && !cancellationToken.IsCancellationRequested)
        {
            var segment = new ArraySegment<byte>(buffer);
            WebSocketReceiveResult result;
            using var ms = new System.IO.MemoryStream();

            do
            {
                result = await socket.ReceiveAsync(segment, cancellationToken);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", cancellationToken);
                    return;
                }

                await ms.WriteAsync(buffer.AsMemory(0, result.Count), cancellationToken);
            } while (!result.EndOfMessage);

            var messageBytes = ms.ToArray();
            var messageText = Encoding.UTF8.GetString(messageBytes);
        }
    }
}