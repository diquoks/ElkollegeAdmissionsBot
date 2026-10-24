using MaxBotCommand = Max.Bot.Types.BotCommand;
using TelegramBotCommand = Telegram.BotAPI.AvailableTypes.BotCommand;

namespace ElkollegeAdmissionsBot.Models.Bot;

public record BotCommand(string Name, string Description)
{
    public MaxBotCommand ToMax() => new(Name, Description);

    public TelegramBotCommand ToTelegram() => new(Name, Description);
}
