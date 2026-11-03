using ElkollegeAdmissionsBot.Models.Bot;

namespace ElkollegeAdmissionsBot.Helpers;

public static class BotButtons
{
    public static BotCallbackButton AskCustomQuestion =>
        InternalAskCustomQuestion("Задать вопрос");

    public static BotCallbackButton ExpandCustomQuestion =>
        InternalAskCustomQuestion("Дополнить вопрос");

    public static BotCallbackButton ReplyAsCustomQuestion =>
        InternalAskCustomQuestion("Ответить");

    private static BotCallbackButton InternalAskCustomQuestion(string text) => new(
        text,
        CallbackStrings.AskCustomQuestion);

    public static BotCallbackButton CancelToStart => new(
        "Отмена",
        CallbackStrings.Start);
}
