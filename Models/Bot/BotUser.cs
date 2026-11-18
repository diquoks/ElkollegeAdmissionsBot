using MaxUser = Max.Bot.Types.User;
using TelegramUser = Telegram.BotAPI.AvailableTypes.User;

namespace ElkollegeAdmissionsBot.Models.Bot;

public record BotUser
{
    public required long Id { get; init; }

    public required string? Username { get; init; }

    public required string FirstName { get; init; }

    public required string? LastName { get; init; }

    public static BotUser FromMax(MaxUser user) => new()
    {
        Id = user.Id,
        Username = user.Username,
        FirstName = user.FirstName,
        LastName = user.LastName
    };

    public static BotUser FromTelegram(TelegramUser user) => new()
    {
        Id = user.Id,
        Username = user.Username,
        FirstName = user.FirstName,
        LastName = user.LastName
    };

    public string FullName => !string.IsNullOrWhiteSpace(LastName) ? $"{FirstName} {LastName}" : FirstName;
}
