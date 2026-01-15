using System;
using System.Threading;
using System.Threading.Tasks;
using Signal.Bot.Types;

namespace Signal.Bot.Polling;

public class DefaultUpdateHandler(
    Func<ISignalBotClient, ReceivedMessage, CancellationToken, Task> updateHandler,
    Func<ISignalBotClient, Exception, HandleErrorSource, CancellationToken, Task> errorHandler) : IUpdateHandler
{
    public DefaultUpdateHandler(
        Func<ISignalBotClient, ReceivedMessage, CancellationToken, Task> updateHandler,
        Func<ISignalBotClient, Exception, CancellationToken, Task> errorHandler)
        : this(updateHandler, (bot, ex, s, ct) => errorHandler(bot, ex, ct))
    {
    }

    public async Task HandleUpdateAsync(ISignalBotClient botClient, ReceivedMessage update,
        CancellationToken cancellationToken)
        => await updateHandler(botClient, update, cancellationToken).ConfigureAwait(false);

    public async Task HandleErrorAsync(ISignalBotClient botClient, Exception exception, HandleErrorSource source,
        CancellationToken cancellationToken)
        => await errorHandler(botClient, exception, source, cancellationToken).ConfigureAwait(false);
}