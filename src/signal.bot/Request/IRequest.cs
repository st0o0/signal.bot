using System.Net.Http;

namespace signal.bot.Request;

public interface IRequest
{
    HttpMethod HttpMethod { get; set; }
    string MethodName { get; }
    HttpContent? ToHttpContent();
}