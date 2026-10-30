using MaxMessage = Max.Bot.Types.Message;
using TelegramMessage = Telegram.BotAPI.AvailableTypes.Message;

namespace ElkollegeAdmissionsBot.Models.Bot;

public record BotMessage(string Id, long ChatId, BotUser? From, string? Text)
{
    public static BotMessage FromMax(MaxMessage message) => new(
        Id: message.Mid!,
        ChatId: message.Recipient!.ChatId!.Value,
        From: message.Sender is not null ? BotUser.FromMax(message.Sender) : null,
        Text: message.Text);

    public static BotMessage FromTelegram(TelegramMessage message) => new(
        Id: message.MessageId.ToString(),
        ChatId: message.Chat.Id,
        From: message.From is not null ? BotUser.FromTelegram(message.From) : null,
        Text: message.Text);

    public int? IntId => int.TryParse(Id, out var result) ? result : null;
}
