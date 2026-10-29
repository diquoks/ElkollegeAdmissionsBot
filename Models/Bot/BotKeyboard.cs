using MaxInlineKeyboard = Max.Bot.Types.InlineKeyboard;
using TelegramInlineKeyboard = Telegram.BotAPI.AvailableTypes.InlineKeyboardMarkup;

namespace ElkollegeAdmissionsBot.Models.Bot;

public record BotKeyboard(IBotButton[][] Rows)
{
    public MaxInlineKeyboard ToMax() =>
        new([.. Rows.Select(row => row.Select(button => button.ToMax()).ToArray())]);

    public TelegramInlineKeyboard ToTelegram() =>
        new([.. Rows.Select(row => row.Select(button => button.ToTelegram()).ToArray())]);
}
