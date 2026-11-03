using ElkollegeAdmissionsBot.Models.Bot;

namespace ElkollegeAdmissionsBot.Models.Database;

public record DatabaseMessage
{
    public required string Id { get; init; }

    public required MessengerType MessengerType { get; init; }

    public required string OriginalId { get; init; }

    public required long OriginalChatId { get; init; }

    public static DatabaseMessage FromForwardedMessage(BotMessage originalMessage, BotMessage forwardedMessage) => new()
    {
        Id = forwardedMessage.Id,
        MessengerType = originalMessage.MessengerType,
        OriginalId = originalMessage.Id,
        OriginalChatId = originalMessage.ChatId
    };
}
