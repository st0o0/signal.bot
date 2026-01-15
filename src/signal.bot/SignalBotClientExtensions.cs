using System.Threading;
using System.Threading.Tasks;
using Signal.Bot.Requests;
using Signal.Bot.Types;

namespace Signal.Bot;

public static partial class SignalBotClientExtensions
{
    public static Task<SendMessage> SendMessageAsync(
        this ISignalBotClient client,
        string recipient,
        string message,
        string? senderNumber = null,
        CancellationToken cancellationToken = default)
    {
        return client.SendRequestAsync(new SendMessageRequest
        {
            Recipients = new List<string> { recipient },
            Message = message,
            Number = senderNumber
        }, cancellationToken: cancellationToken);
    }

    public static Task<About> GetAboutAsync(
        this ISignalBotClient client,
        CancellationToken cancellationToken = default)
    {
        return client.SendRequestAsync(new GetAboutRequest(), cancellationToken: cancellationToken);
    }

    public static Task<ICollection<Account>> GetAccountsAsync(
        this ISignalBotClient client,
        CancellationToken cancellationToken = default)
    {
        return client.SendRequestAsync(new GetAccountsRequest(), cancellationToken: cancellationToken);
    }

    public static Task<ICollection<Group>> GetGroupsAsync(
        this ISignalBotClient client,
        string number,
        CancellationToken cancellationToken = default)
    {
        return client.SendRequestAsync(new GetGroupsRequest(number), cancellationToken: cancellationToken);
    }

    public static Task RegisterNumberAsync(
        this ISignalBotClient client,
        string number,
        string? captcha = null,
        bool? useVoice = null,
        CancellationToken cancellationToken = default)
    {
        return client.SendRequestAsync(new RegisterNumberRequest(number)
        {
            Captcha = captcha,
            UseVoice = useVoice
        }, cancellationToken: cancellationToken);
    }

    public static Task<string> VerifyNumberAsync(
        this ISignalBotClient client,
        string number,
        string token,
        string? pin = null,
        CancellationToken cancellationToken = default)
    {
        return client.SendRequestAsync(new VerifyNumberRequest(number, token)
        {
            Pin = pin
        }, cancellationToken: cancellationToken);
    }

    public static Task UpdateProfileAsync(
        this ISignalBotClient client,
        string number,
        string? name = null,
        string? about = null,
        string? base64Avatar = null,
        CancellationToken cancellationToken = default)
    {
        return client.SendRequestAsync(new UpdateProfileRequest(number)
        {
            Name = name,
            About = about,
            Base64Avatar = base64Avatar
        }, cancellationToken: cancellationToken);
    }

    public static Task SetTypingIndicatorAsync(
        this ISignalBotClient client,
        string number,
        string? recipient = null,
        string? groupId = null,
        bool isTyping = true,
        CancellationToken cancellationToken = default)
    {
        var typing = new SetTypingIndicatorRequest(number)
        {
            GroupId = groupId,
            Recipient = recipient
        };
        var resetTyping = new RemoveTypingIndicatorRequest(number)
        {
            GroupId = groupId,
            Recipient = recipient
        };
        return client.SendRequestAsync(isTyping ? typing : resetTyping, cancellationToken: cancellationToken);
    }

    // Account extensions
    public static Task SetPinAsync(
        this ISignalBotClient client,
        string number,
        string pin,
        CancellationToken cancellationToken = default)
    {
        return client.SendRequestAsync(new SetPinRequest(number) { Pin = pin },
            cancellationToken: cancellationToken);
    }

    public static Task RemovePinAsync(
        this ISignalBotClient client,
        string number,
        CancellationToken cancellationToken = default)
    {
        return client.SendRequestAsync(new RemovePinRequest(number), cancellationToken: cancellationToken);
    }

    public static Task RateLimitChallengeAsync(
        this ISignalBotClient client,
        string number,
        string challengeToken,
        string captcha,
        CancellationToken cancellationToken = default)
    {
        return client.SendRequestAsync(new RateLimitChallengeRequest(number)
        {
            ChallengeToken = challengeToken,
            Captcha = captcha
        }, cancellationToken: cancellationToken);
    }

    public static Task UpdateAccountSettingsAsync(
        this ISignalBotClient client,
        string number,
        bool discoverableByNumber = true,
        bool shareNumberWithContacts = true,
        CancellationToken cancellationToken = default)
    {
        return client.SendRequestAsync(new UpdateAccountSettingsRequest(number)
        {
            DiscoverableByNumber = discoverableByNumber,
            ShareNumberWithContacts = shareNumberWithContacts
        }, cancellationToken: cancellationToken);
    }

    public static Task<SetUsername> SetUsernameAsync(
        this ISignalBotClient client,
        string number,
        string username,
        CancellationToken cancellationToken = default)
    {
        return client.SendRequestAsync(new SetUsernameRequest(number) { Username = username },
            cancellationToken: cancellationToken);
    }

    public static Task RemoveUsernameAsync(
        this ISignalBotClient client,
        string number,
        CancellationToken cancellationToken = default)
    {
        return client.SendRequestAsync(new RemoveUsernameRequest(number), cancellationToken: cancellationToken);
    }

    // Device extensions
    public static Task<ICollection<Device>> GetDevicesAsync(
        this ISignalBotClient client,
        string number,
        CancellationToken cancellationToken = default)
    {
        return client.SendRequestAsync(new GetDevicesRequest(number), cancellationToken: cancellationToken);
    }

    public static Task AddDeviceAsync(
        this ISignalBotClient client,
        string number,
        string uri,
        CancellationToken cancellationToken = default)
    {
        return client.SendRequestAsync(new AddDeviceRequest(number) { Uri = uri },
            cancellationToken: cancellationToken);
    }

    public static Task UnregisterDeviceAsync(
        this ISignalBotClient client,
        string number,
        CancellationToken cancellationToken = default)
    {
        return client.SendRequestAsync(new UnregisterDeviceRequest(number), cancellationToken: cancellationToken);
    }

    // Content extensions
    public static Task<ICollection<string>> GetAttachmentsAsync(
        this ISignalBotClient client,
        CancellationToken cancellationToken = default)
    {
        return client.SendRequestAsync(new GetAttachmentsRequest(), cancellationToken: cancellationToken);
    }

    public static Task<string> GetAttachmentAsync(
        this ISignalBotClient client,
        string attachmentId,
        CancellationToken cancellationToken = default)
    {
        return client.SendRequestAsync(new GetAttachmentRequest(attachmentId), cancellationToken: cancellationToken);
    }

    public static Task RemoveAttachmentAsync(
        this ISignalBotClient client,
        string attachmentId,
        CancellationToken cancellationToken = default)
    {
        return client.SendRequestAsync(new RemoveAttachmentRequest(attachmentId), cancellationToken: cancellationToken);
    }

    public static Task AddReactionAsync(
        this ISignalBotClient client,
        string number,
        string reaction,
        string recipient,
        string targetAuthor,
        int timestamp,
        CancellationToken cancellationToken = default)
    {
        return client.SendRequestAsync(new AddReactionRequest(number)
        {
            Reaction = reaction,
            Recipient = recipient,
            TargetAuthor = targetAuthor,
            Timestamp = timestamp
        }, cancellationToken: cancellationToken);
    }

    public static Task<string> RemoveReactionAsync(
        this ISignalBotClient client,
        string number,
        string emoji,
        string recipient,
        string targetAuthor,
        int timestamp,
        CancellationToken cancellationToken = default)
    {
        return client.SendRequestAsync(new RemoveReactionRequest(number)
        {
            Emoji = emoji,
            Recipient = recipient,
            TargetAuthor = targetAuthor,
            Timestamp = timestamp
        }, cancellationToken: cancellationToken);
    }

    public static Task<RemoteDelete> RemoteDeleteAsync(
        this ISignalBotClient client,
        string number,
        string recipient,
        int timestamp,
        CancellationToken cancellationToken = default)
    {
        return client.SendRequestAsync(new RemoteDeleteRequest(number)
        {
            Recipient = recipient,
            Timestamp = timestamp
        }, cancellationToken: cancellationToken);
    }

    public static Task<ICollection<InstalledStickerPack>> GetStickerPacksAsync(
        this ISignalBotClient client,
        string number,
        CancellationToken cancellationToken = default)
    {
        return client.SendRequestAsync(new GetStickerPacksRequest(number), cancellationToken: cancellationToken);
    }

    public static Task AddStickerPackAsync(
        this ISignalBotClient client,
        string number,
        string packId,
        string packKey,
        CancellationToken cancellationToken = default)
    {
        return client.SendRequestAsync(new AddStickerPackRequest(number)
        {
            PackId = packId,
            PackKey = packKey
        }, cancellationToken: cancellationToken);
    }

    // Social extensions
    public static Task<ICollection<Contact>> GetContactsAsync(
        this ISignalBotClient client,
        string number,
        CancellationToken cancellationToken = default)
    {
        return client.SendRequestAsync(new GetContactsRequest(number), cancellationToken: cancellationToken);
    }

    public static Task UpdateContactAsync(
        this ISignalBotClient client,
        string number,
        string recipient,
        string? name = null,
        int? expirationTime = null,
        CancellationToken cancellationToken = default)
    {
        return client.SendRequestAsync(new UpdateContactRequest(number)
        {
            Recipient = recipient,
            Name = name,
            ExpirationTime = expirationTime
        }, cancellationToken: cancellationToken);
    }

    public static Task SyncContactsAsync(
        this ISignalBotClient client,
        string number,
        CancellationToken cancellationToken = default)
    {
        return client.SendRequestAsync(new SyncContactsRequest(number), cancellationToken: cancellationToken);
    }

    public static Task<Group> GetGroupAsync(
        this ISignalBotClient client,
        string number,
        string groupId,
        CancellationToken cancellationToken = default)
    {
        return client.SendRequestAsync(new GetGroupRequest(number, groupId), cancellationToken: cancellationToken);
    }

    public static Task RemoveGroupAsync(
        this ISignalBotClient client,
        string number,
        string groupId,
        CancellationToken cancellationToken = default)
    {
        return client.SendRequestAsync(new RemoveGroupRequest(number, groupId), cancellationToken: cancellationToken);
    }

    public static Task AddGroupAdminAsync(
        this ISignalBotClient client,
        string number,
        string groupId,
        ICollection<string> admins,
        CancellationToken cancellationToken = default)
    {
        return client.SendRequestAsync(new AddGroupAdminRequest(number, groupId) { Admins = admins },
            cancellationToken: cancellationToken);
    }

    public static Task RemoveGroupAdminAsync(
        this ISignalBotClient client,
        string number,
        string groupId,
        ICollection<string> admins,
        CancellationToken cancellationToken = default)
    {
        return client.SendRequestAsync(new RemoveGroupAdminRequest(number, groupId) { Admins = admins },
            cancellationToken: cancellationToken);
    }

    public static Task AddGroupMemberAsync(
        this ISignalBotClient client,
        string number,
        string groupId,
        ICollection<string> members,
        CancellationToken cancellationToken = default)
    {
        return client.SendRequestAsync(new AddGroupMemberRequest(number, groupId) { Members = members },
            cancellationToken: cancellationToken);
    }

    public static Task RemoveGroupMemberAsync(
        this ISignalBotClient client,
        string number,
        string groupId,
        ICollection<string> members,
        CancellationToken cancellationToken = default)
    {
        return client.SendRequestAsync(new RemoveGroupMemberRequest(number, groupId) { Members = members },
            cancellationToken: cancellationToken);
    }

    public static Task QuitGroupAsync(
        this ISignalBotClient client,
        string number,
        string groupId,
        CancellationToken cancellationToken = default)
    {
        return client.SendRequestAsync(new QuitGroupRequest(number, groupId), cancellationToken: cancellationToken);
    }

    public static Task<ICollection<Identity>> GetIdentitiesAsync(
        this ISignalBotClient client,
        string number,
        CancellationToken cancellationToken = default)
    {
        return client.SendRequestAsync(new GetIdentitiesRequest(number), cancellationToken: cancellationToken);
    }

    public static Task TrustIdentityAsync(
        this ISignalBotClient client,
        string number,
        string verifiedNumber,
        bool? trustAllKnownKeys = null,
        string? verifiedSafetyNumber = null,
        CancellationToken cancellationToken = default)
    {
        return client.SendRequestAsync(new TrustIdentityRequest(number, verifiedNumber)
        {
            TrustAllKnownKeys = trustAllKnownKeys,
            VerifiedSafetyNumber = verifiedSafetyNumber
        }, cancellationToken: cancellationToken);
    }

    public static Task<ICollection<SearchResponse>> SearchNumbersAsync(
        this ISignalBotClient client,
        string number,
        IEnumerable<string> numbers,
        CancellationToken cancellationToken = default)
    {
        return client.SendRequestAsync(new SearchNumbersRequest(number)
        {
            Numbers = numbers as ICollection<string> ?? new List<string>(numbers)
        }, cancellationToken: cancellationToken);
    }

    // Configuration extensions
    public static Task<Configuration> GetConfigurationAsync(
        this ISignalBotClient client,
        CancellationToken cancellationToken = default)
    {
        return client.SendRequestAsync(new GetConfigurationRequest(), cancellationToken: cancellationToken);
    }

    public static Task SetConfigurationAsync(
        this ISignalBotClient client,
        string logging,
        CancellationToken cancellationToken = default)
    {
        return client.SendRequestAsync(new SetConfigurationRequest { Logging = logging },
            cancellationToken: cancellationToken);
    }
}