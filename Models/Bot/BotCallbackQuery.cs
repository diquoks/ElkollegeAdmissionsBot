using MaxCallbackQuery = Max.Bot.Types.CallbackQuery;
using MaxMessage = Max.Bot.Types.Message;
using TelegramCallbackQuery = Telegram.BotAPI.AvailableTypes.CallbackQuery;

namespace ElkollegeAdmissionsBot.Models.Bot;

public record BotCallbackQuery : BotUpdate
{
    public required string Id { get; init; }

    public required string Data { get; init; }

    public required string MessageId { get; init; }

    public static BotCallbackQuery FromMax(MaxCallbackQuery callbackQuery, MaxMessage message) => new()
    {
        Id = callbackQuery.CallbackId,
        MessengerType = BotMessengerType.Max,
        ChatId = message.Recipient.ChatId!.Value,
        From = BotUser.FromMax(callbackQuery.User),
        Data = callbackQuery.Payload!,
        MessageId = message.Mid!
    };

    public static BotCallbackQuery FromTelegram(TelegramCallbackQuery callbackQuery) => new()
    {
        Id = callbackQuery.Id,
        MessengerType = BotMessengerType.Telegram,
        ChatId = callbackQuery.Message!.Chat.Id,
        From = BotUser.FromTelegram(callbackQuery.From),
        Data = callbackQuery.Data!,
        MessageId = callbackQuery.Message.MessageId.ToString()
    };
}
