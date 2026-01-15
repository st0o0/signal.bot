using System;
using System.Threading;
using System.Threading.Tasks;
using Signal.Bot.Types;

namespace Signal.Bot.Polling;

public interface IUpdateHandler
{
    /// <summary>Handles an <see cref="ReceivedMessage"/></summary>
    /// <param name="botClient">The <see cref="ISignalBotClient"/> instance of the bot receiving the <see cref="Update"/></param>
    /// <param name="message">The <see cref="ReceivedMessage"/> to handle</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> which will notify that method execution should be cancelled</param>
    Task HandleUpdateAsync(ISignalBotClient botClient, ReceivedMessage message, CancellationToken cancellationToken);

    /// <summary>Handles an <see cref="Exception"/></summary>
    /// <param name="botClient">The <see cref="ISignalBotClient"/> instance of the bot receiving the <see cref="Exception"/></param>
    /// <param name="exception">The <see cref="Exception"/> to handle</param>
    /// <param name="source">Where the error occured</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> which will notify that method execution should be cancelled</param>
    Task HandleErrorAsync(ISignalBotClient botClient, Exception exception, HandleErrorSource source,
        CancellationToken cancellationToken);
}