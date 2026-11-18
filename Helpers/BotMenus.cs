using ElkollegeAdmissionsBot.Models.Bot;
using ElkollegeAdmissionsBot.Models.FAQ;

namespace ElkollegeAdmissionsBot.Helpers;

public static class BotMenus
{
    public static BotMenu Default => new(
        """
        <b>Извините, я не могу ответить на это сообщение.</b>

        Вы можете поискать ответ на свой вопрос в <b>главном меню</b>, или <b>написать сообщение</b> для приёмной комиссии.
        Воспользуйтесь кнопками ниже:
        """,
        new BotKeyboardBuilder()
            .Append(BotButtons.Start)
            .AppendRow()
            .Append(BotButtons.WriteCustomQuestion)
            .Build());

    public static BotMenu Start(string botName, IReadOnlyDictionary<string, FrequentlyAskedTopic> faqTopics,
        bool withCustomQuestionButton)
    {
        var keyboardBuilder = new BotKeyboardBuilder();

        if (faqTopics.Count > 0)
        {
            foreach (var pair in faqTopics)
                keyboardBuilder
                    .AppendRow()
                    .Append(BotButtons.Topic(pair.Value.Title, pair.Key));

            if (withCustomQuestionButton)
                keyboardBuilder
                    .AppendRow()
                    .Append(BotButtons.EllipsisJustAnswer);
        }

        if (withCustomQuestionButton)
            keyboardBuilder
                .AppendRow()
                .Append(BotButtons.AskCustomQuestion);

        return new BotMenu(
            $"""
             <b>Привет, я {botName}!</b>

             Здесь вы сможете найти ответы на часто задаваемые вопросы, а также задать свой вопрос приёмной комиссии.

             Выберите нужную тему из списка ниже:
             """,
            keyboardBuilder.Build());
    }

    public static BotMenu Topic(FrequentlyAskedTopic faqTopic, string faqTopicKey,
        IReadOnlyDictionary<string, FrequentlyAskedSection> faqSections, bool withCustomQuestionButton)
    {
        var keyboardBuilder = new BotKeyboardBuilder();

        if (faqSections.Count > 0)
        {
            foreach (var pair in faqSections)
                keyboardBuilder
                    .AppendRow()
                    .Append(BotButtons.Section(pair.Value.Title, faqTopicKey, pair.Key));

            keyboardBuilder
                .AppendRow()
                .Append(BotButtons.EllipsisJustAnswer);
        }

        if (withCustomQuestionButton)
            keyboardBuilder
                .AppendRow()
                .Append(BotButtons.TopicCustomQuestion);

        keyboardBuilder
            .AppendRow()
            .Append(BotButtons.BackTo(CallbackStrings.StartCallback));

        return new BotMenu(
            $"""
             <b>Тема «{faqTopic.Title}»</b>

             О чём ваш вопрос?
             Выберите подходящий раздел из списка ниже:
             """,
            keyboardBuilder.Build());
    }

    public static BotMenu Section(FrequentlyAskedSection faqSection, string faqTopicKey, bool withCustomQuestionButton)
    {
        var keyboardBuilder = new BotKeyboardBuilder();

        if (faqSection.Links is not null && faqSection.Links.Length > 0)
        {
            foreach (var faqSectionLink in faqSection.Links)
                keyboardBuilder
                    .AppendRow()
                    .Append(BotButtons.SectionLink(faqSectionLink));

            keyboardBuilder
                .AppendRow()
                .Append(BotButtons.EllipsisJustAnswer);
        }

        if (withCustomQuestionButton)
            keyboardBuilder
                .AppendRow()
                .Append(BotButtons.SectionCustomQuestion);

        keyboardBuilder
            .AppendRow()
            .Append(BotButtons.BackTo(CallbackStrings.TopicCallback(faqTopicKey)));

        return new BotMenu(
            $"""
             <b>Раздел «{faqSection.Title}»</b>

             {faqSection.Text}
             """,
            keyboardBuilder.Build());
    }

    public static BotMenu AskCustomQuestion => new(
        "<b>Задайте свой вопрос приёмной комиссии:</b>",
        new BotKeyboardBuilder()
            .Append(BotButtons.CancelTo(CallbackStrings.StartCallback))
            .Build());

    public static BotMenu CustomQuestionAsked => new(
        """
        <b>Вопрос отправлен!</b>

        Ответное сообщение будет переслано в этот чат.
        """,
        new BotKeyboardBuilder()
            .Append(BotButtons.ExpandCustomQuestion)
            .Build());

    public static BotMenu AnswerFromAdminGroup(string text, bool withCustomQuestionButton) => new(
        $"""
         <b>Ответ от приёмной комиссии:</b>

         «{text}»
         """,
        withCustomQuestionButton
            ? new BotKeyboardBuilder()
                .Append(BotButtons.ReplyAsCustomQuestion)
                .Build()
            : null);

    public static BotMenu ForwardToAdminGroup(BotMessage message) => new(
        $"""
         <b>Новое сообщение из {message.MessengerType}:</b>

         «{message.Text}»
         Отправитель: {SenderReadableInfo(message.From)}
         """,
        new BotKeyboardBuilder()
            .Append(BotButtons.BlockUser(message.From.Id, message.MessengerType))
            .Build());

    public static BotMenu UserBlockedSuccessfully(BotUser user) => new(
        $"<b>{SenderReadableInfo(user)} заблокирован!</b>");

    public static BotMenu UserAlreadyBlocked(BotUser user) => new(
        $"<b>{SenderReadableInfo(user)} уже заблокирован!</b>");

    private static string SenderReadableInfo(BotUser sender) =>
        $"{sender.FullName} (<pre>{sender.Id}</pre>)";
}
