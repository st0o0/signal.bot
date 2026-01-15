namespace Signal.Bot.Types;

public class Profile
{
    [JsonPropertyName("about")] public string? About { get; set; } = null;

    [JsonPropertyName("given_name")] public string? GivenName { get; set; } = null;

    [JsonPropertyName("has_avatar")] public bool? HasAvatar { get; set; } = null;

    [JsonPropertyName("last_updated_timestamp")]
    public int? LastUpdatedTimestamp { get; set; } = null;

    [JsonPropertyName("lastname")] public string? Lastname { get; set; } = null;
}