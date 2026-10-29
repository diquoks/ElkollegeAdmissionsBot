using MaxMessage = Max.Bot.Types.Message;
using TelegramMessage = Telegram.BotAPI.AvailableTypes.Message;

namespace ElkollegeAdmissionsBot.Models.Bot;

public record BotMessage(string? Id, long? ChatId, string? Text, BotUser? From)
{
    public static BotMessage FromMax(MaxMessage message) => new(
        Id: message.Mid,
        ChatId: message.Recipient?.ChatId,
        Text: message.Text,
        From: message.Sender != null ? BotUser.FromMax(message.Sender) : null);

    public static BotMessage FromTelegram(TelegramMessage message) => new(
        Id: message.MessageId.ToString(),
        ChatId: message.Chat.Id,
        Text: message.Text,
        From: message.From != null ? BotUser.FromTelegram(message.From) : null);

    public int? IntId => int.TryParse(Id ?? string.Empty, out var result) ? result : null;
}
