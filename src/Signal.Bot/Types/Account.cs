namespace Signal.Bot.Types;

public class Account
{
    [JsonPropertyName("number")] public string? Number { get; set; } = null;

    [JsonPropertyName("uuid")] public string? Uuid { get; set; } = null;
}