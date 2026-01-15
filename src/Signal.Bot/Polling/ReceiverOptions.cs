using System;

namespace Signal.Bot.Polling;

public sealed class ReceiverOptions
{
    public int? Limit
    {
        get;
        set => field = value is not < 1 and not > 100
            ? value
            : throw new ArgumentOutOfRangeException(nameof(value), value,
                $"'{nameof(Limit)}' can not be less than 1 or greater than 100");
    }

    public bool DropPendingUpdates { get; set; }
}