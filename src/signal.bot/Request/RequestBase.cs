using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace signal.bot.Request;

public abstract class RequestBase(string methodName) : IRequest
{
    [JsonIgnore] public HttpMethod HttpMethod { get; set; } = HttpMethod.Post;

    [JsonIgnore] public string MethodName { get; } = methodName;

    [JsonIgnore] public bool IsWebhookResponse { get; set; }

    public virtual HttpContent ToHttpContent() =>
        System.Net.Http.Json.JsonContent.Create(this, GetType(), options: new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
        });
}