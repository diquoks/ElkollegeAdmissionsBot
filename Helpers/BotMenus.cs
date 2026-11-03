using ElkollegeAdmissionsBot.Models.Bot;

namespace ElkollegeAdmissionsBot.Helpers;

public static class BotMenus
{
    public static BotMenu Start(string botName, bool addCustomQuestionButton)
    {
        var keyboardBuilder = new BotKeyboardBuilder();

        if (addCustomQuestionButton)
            keyboardBuilder.Append(BotButtons.AskCustomQuestion);

        return new BotMenu(
            $"""
             <b>Привет, я {botName}!</b>

             Здесь вы сможете найти ответы на часто задаваемые вопросы, а также задать свой вопрос приёмной комиссии.
             """,
            keyboardBuilder.Build());
    }

    public static BotMenu AskCustomQuestion => new(
        "<b>Задайте свой вопрос приёмной комиссии:</b>",
        new BotKeyboardBuilder()
            .Append(BotButtons.CancelToStart)
            .Build());

    public static BotMenu CustomQuestionAsked => new(
        """
        <b>Вопрос отправлен!</b>

        Ответное сообщение будет переслано в этот чат.
        """,
        new BotKeyboardBuilder()
            .Append(BotButtons.ExpandCustomQuestion)
            .Build());

    public static BotMenu ForwardToAdminGroup(BotMessage message) => new(
        $"""
         <b>Новое сообщение из {message.MessengerType}:</b>
         "{message.Text}"

         Отправитель: {message.From.FullName} (<pre>{message.From.Id}</pre>)
         """);

    public static BotMenu AnswerFromAdminGroup(string text) => new(
        $"""
         <b>Ответ от приёмной комиссии:</b>
         "{text}"
         """,
        new BotKeyboardBuilder()
            .Append(BotButtons.ReplyAsCustomQuestion)
            .Build());
}
