namespace Signal.Bot.Types;

public class Device
{
    [JsonPropertyName("id")] public int Id { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("created")] public long Created { get; set; }

    [JsonPropertyName("lastSeen")] public long LastSeen { get; set; }
}