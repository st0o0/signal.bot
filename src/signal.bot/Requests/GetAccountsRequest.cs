using Signal.Bot.Types;

namespace Signal.Bot.Requests;

public class GetAccountsRequest() : RequestBase<ICollection<Account>>("v1/accounts")
{
    public override HttpMethod HttpMethod => HttpMethod.Get;
}