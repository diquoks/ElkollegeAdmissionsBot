using MaxUser = Max.Bot.Types.User;
using TelegramUser = Telegram.BotAPI.AvailableTypes.User;

namespace ElkollegeAdmissionsBot.Models.Bot;

public record BotUser(string? Username)
{
    public static BotUser FromMax(MaxUser user) => new(user.Username);

    public static BotUser FromTelegram(TelegramUser user) => new(user.Username);
}
