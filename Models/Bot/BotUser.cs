using MaxUser = Max.Bot.Types.User;
using TelegramUser = Telegram.BotAPI.AvailableTypes.User;

namespace ElkollegeAdmissionsBot.Models.Bot;

public record BotUser(long Id, string? Username, string FirstName, string? LastName)
{
    public static BotUser FromMax(MaxUser user) => new(
        Id: user.Id,
        Username: user.Username,
        FirstName: user.FirstName,
        LastName: user.LastName);

    public static BotUser FromTelegram(TelegramUser user) => new(
        Id: user.Id,
        Username: user.Username,
        FirstName: user.FirstName,
        LastName: user.LastName);

    public string FullName => !string.IsNullOrWhiteSpace(LastName) ? $"{FirstName} {LastName}" : FirstName;

    public string ReadableInfo => !string.IsNullOrWhiteSpace(Username) ? $"{FullName} | @{Username}" : FullName;
}
