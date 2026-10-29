using ElkollegeAdmissionsBot.Models.Bot;

namespace ElkollegeAdmissionsBot.Helpers;

public static class BotCommands
{
    private const string StartCommand = "start";

    public static BotCommand[] AllCommands { get; } =
    [
        new(StartCommand, "Получить ответы на часто задаваемые вопросы")
    ];
}
