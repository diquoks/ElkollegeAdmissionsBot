using MaxInlineKeyboardButton = Max.Bot.Types.InlineKeyboardButton;
using TelegramInlineKeyboardButton = Telegram.BotAPI.AvailableTypes.InlineKeyboardButton;

namespace ElkollegeAdmissionsBot.Models.Bot;

public interface IBotButton
{
    public MaxInlineKeyboardButton ToMax();

    public TelegramInlineKeyboardButton ToTelegram();
}

public record BotCallbackButton(string Text, string Callback) : IBotButton
{
    public MaxInlineKeyboardButton ToMax() => new() { Text = Text, CallbackData = Callback };

    public TelegramInlineKeyboardButton ToTelegram() => new(Text) { CallbackData = Callback };
}

public record BotLinkButton(string Text, string Url) : IBotButton
{
    public MaxInlineKeyboardButton ToMax() => new() { Text = Text, Url = Url };

    public TelegramInlineKeyboardButton ToTelegram() => new(Text) { Url = Url };
}
