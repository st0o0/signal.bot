# signal.bot

A Signal Bot Client for .NET, inspired by the Telegram.Bot library.

## Usage

```csharp
var client = new SignalBotClient("http://localhost:8080/");

// Send a message
await client.SendMessageAsync("+123456789", "Hello from Signal Bot!");

// Start receiving messages
using var cts = new CancellationTokenSource();
await client.StartReceiving(
    "+123456789",
    async (botClient, message, ct) => {
        Console.WriteLine($"Received: {message}");
    },
    async (botClient, exception, ct) => {
        Console.WriteLine($"Error: {exception.Message}");
    },
    cts.Token
);
```

## Features

- Request-Response pattern for all Signal API operations.
- Convenience extension methods.
- Polling mechanism for receiving messages (`StartReceiving`).
- Supports the full Signal Cli REST API.