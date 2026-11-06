using MaxMessage = Max.Bot.Types.Message;
using TelegramMessage = Telegram.BotAPI.AvailableTypes.Message;

namespace ElkollegeAdmissionsBot.Models.Bot;

public record BotMessage(
    string Id,
    long ChatId,
    BotMessengerType MessengerType,
    BotUser From,
    string Text,
    string? LinkedMessageId = null)
{
    public static BotMessage FromMax(MaxMessage message) => new(
        Id: message.Mid!,
        ChatId: message.Recipient.ChatId!.Value,
        MessengerType: BotMessengerType.Max,
        From: BotUser.FromMax(message.Sender!),
        Text: message.Text!,
        LinkedMessageId: message.Link?.Mid);

    public static BotMessage FromTelegram(TelegramMessage message) => new(
        Id: message.MessageId.ToString(),
        ChatId: message.Chat.Id,
        MessengerType: BotMessengerType.Telegram,
        From: BotUser.FromTelegram(message.From!),
        Text: message.Text!,
        LinkedMessageId: message.ReplyToMessage?.MessageId.ToString());
}
