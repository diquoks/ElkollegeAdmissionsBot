using ElkollegeAdmissionsBot.Models.Bot;
using ElkollegeAdmissionsBot.Models.FAQ;

namespace ElkollegeAdmissionsBot.Helpers;

public static class BotButtons
{
    public static BotCallbackButton Topic(string title, string key) => new(
        title,
        CallbackStrings.TopicCallback(key));

    public static BotCallbackButton TopicCustomQuestion =>
        InternalAskCustomQuestion("Нет нужной темы?");

    public static BotCallbackButton Section(string title, string key, string topicKey) => new(
        title,
        CallbackStrings.SectionCallback(key, topicKey));

    public static BotLinkButton SectionLink(FrequentlyAskedSectionLink faqSectionLink) => new(
        faqSectionLink.Title,
        faqSectionLink.Url);

    public static BotCallbackButton SectionCustomQuestion =>
        InternalAskCustomQuestion("Не нашли ответ?");

    public static BotCallbackButton AskCustomQuestion =>
        InternalAskCustomQuestion("Задать свой вопрос");

    public static BotCallbackButton ExpandCustomQuestion =>
        InternalAskCustomQuestion("Дополнить вопрос");

    public static BotCallbackButton ReplyAsCustomQuestion =>
        InternalAskCustomQuestion("Ответить");

    private static BotCallbackButton InternalAskCustomQuestion(string text) => new(
        text,
        CallbackStrings.AskCustomQuestionCallback);

    public static BotCallbackButton EllipsisJustAnswer => new(
        "...",
        CallbackStrings.JustAnswerCallback);

    public static BotCallbackButton BackTo(string callback) => new(
        "Назад",
        callback);

    public static BotCallbackButton CancelTo(string callback) => new(
        "Отмена",
        callback);
}
