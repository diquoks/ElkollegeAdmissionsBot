using MaxUpdate = Max.Bot.Types.Update;

namespace ElkollegeAdmissionsBot.Models.Bot;

public record BotUpdate
{
    public required BotMessengerType MessengerType { get; init; }

    public required long ChatId { get; init; }

    public required BotUser From { get; init; }

    public static BotUpdate FromMax(MaxUpdate update) => new()
    {
        MessengerType = BotMessengerType.Max,
        ChatId = update.ChatId!.Value,
        From = BotUser.FromMax(update.User!)
    };
}
