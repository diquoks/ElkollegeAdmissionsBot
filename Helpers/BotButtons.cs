using ElkollegeAdmissionsBot.Models.Bot;

namespace ElkollegeAdmissionsBot.Helpers;

public static class BotButtons
{
    public static BotCallbackButton AskCustomQuestion => new(
        "Задать вопрос",
        CallbackStrings.AskCustomQuestion);

    public static BotCallbackButton CancelToStart => new(
        "Отмена",
        CallbackStrings.Start);
}
