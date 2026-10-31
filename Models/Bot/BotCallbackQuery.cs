using MaxCallbackQuery = Max.Bot.Types.CallbackQuery;
using MaxMessage = Max.Bot.Types.Message;
using TelegramCallbackQuery = Telegram.BotAPI.AvailableTypes.CallbackQuery;

namespace ElkollegeAdmissionsBot.Models.Bot;

public record BotCallbackQuery(string Id, string Data, BotUser From, string MessageId, long ChatId)
{
    public static BotCallbackQuery FromMax(MaxCallbackQuery callbackQuery, MaxMessage message) => new(
        Id: callbackQuery.CallbackId,
        Data: callbackQuery.Payload!,
        From: BotUser.FromMax(callbackQuery.User),
        MessageId: message.Mid!,
        ChatId: message.Recipient.ChatId!.Value);

    public static BotCallbackQuery FromTelegram(TelegramCallbackQuery callbackQuery) => new(
        Id: callbackQuery.Id,
        Data: callbackQuery.Data!,
        From: BotUser.FromTelegram(callbackQuery.From),
        MessageId: callbackQuery.Message!.MessageId.ToString(),
        ChatId: callbackQuery.Message.Chat.Id);

    public int? IntMessageId => int.TryParse(MessageId, out var result) ? result : null;
}
