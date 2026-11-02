using ElkollegeAdmissionsBot.Models.Bot;

namespace ElkollegeAdmissionsBot.Helpers;

public static class BotMenus
{
    public static BotMenu Start(string botName) => new(
        $"""
         <b>Привет, я {botName}!</b>

         Здесь ты сможешь найти ответы на часто задаваемые вопросы, а также задать свой вопрос приёмной комиссии.
         """,
        new BotKeyboardBuilder()
            .Append(BotButtons.AskCustomQuestion)
            .Build());

    public static BotMenu AskCustomQuestion => new(
        "<b>Задайте свой вопрос приёмной комиссии:</b>",
        new BotKeyboardBuilder()
            .Append(BotButtons.CancelToStart)
            .Build());

    public static BotMenu CustomQuestionAsked => new(
        """
        <b>Вопрос отправлен!</b>

        Ответное сообщение будет переслано в этот чат.
        """);

    public static BotMenu ForwardToAdminGroup(BotMessage message) => new(
        $"""
         <b>Новое сообщение из {message.MessengerType}:</b>
         "{message.Text}"

         Отправитель: {message.From.FullName} ({message.From.Id})
         """);

    public static BotMenu AnswerFromAdminGroup(string text) => new(
        $"""
         <b>Ответ от приёмной комиссии:</b>
         "{text}"
         """);
}
