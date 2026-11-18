using MaxMessage = Max.Bot.Types.Message;
using TelegramMessage = Telegram.BotAPI.AvailableTypes.Message;

namespace ElkollegeAdmissionsBot.Models.Bot;

public record BotMessage : BotUpdate
{
    public required string Id { get; init; }

    public required string Text { get; init; }

    public required string? LinkedMessageId { get; init; }

    public static BotMessage FromMax(MaxMessage message) => new()
    {
        Id = message.Mid!,
        MessengerType = BotMessengerType.Max,
        ChatId = message.Recipient.ChatId!.Value,
        From = BotUser.FromMax(message.Sender!),
        Text = message.Text!,
        LinkedMessageId = message.Link?.Mid
    };

    public static BotMessage FromTelegram(TelegramMessage message) => new()
    {
        Id = message.MessageId.ToString(),
        MessengerType = BotMessengerType.Telegram,
        ChatId = message.Chat.Id,
        From = BotUser.FromTelegram(message.From!),
        Text = message.Text!,
        LinkedMessageId = message.ReplyToMessage?.MessageId.ToString()
    };
}
