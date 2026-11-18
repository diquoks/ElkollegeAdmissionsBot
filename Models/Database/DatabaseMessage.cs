using ElkollegeAdmissionsBot.Models.Bot;

namespace ElkollegeAdmissionsBot.Models.Database;

public record DatabaseMessage
{
    public required string Id { get; init; }

    public required string OriginalId { get; init; }

    public required BotMessengerType OriginalMessengerType { get; init; }

    public required long OriginalChatId { get; init; }

    public required long OriginalFromId { get; init; }

    public static DatabaseMessage FromForwardedMessage(BotMessage originalMessage, BotMessage forwardedMessage) => new()
    {
        Id = forwardedMessage.Id,
        OriginalId = originalMessage.Id,
        OriginalMessengerType = originalMessage.MessengerType,
        OriginalChatId = originalMessage.ChatId,
        OriginalFromId = originalMessage.From.Id
    };
}
