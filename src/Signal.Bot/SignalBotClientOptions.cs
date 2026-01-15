using System;

namespace Signal.Bot;

public class SignalBotClientOptions
{
    public SignalBotClientOptions(string number, string? httpClientName = null)
    {
        Number = number ?? throw new ArgumentNullException(nameof(number));
        HttpClientName = httpClientName;
    }

    public string? HttpClientName { get; }

    public string Number { get; }
}