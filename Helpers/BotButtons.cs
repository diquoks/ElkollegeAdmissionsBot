using ElkollegeAdmissionsBot.Models.Bot;
using ElkollegeAdmissionsBot.Models.FAQ;

namespace ElkollegeAdmissionsBot.Helpers;

public static class BotButtons
{
    public static BotCallbackButton Start => new(
        "Главное меню",
        CallbackStrings.StartCallback);

    public static BotCallbackButton Topic(string title, string key) => new(
        title,
        CallbackStrings.TopicCallback(key));

    public static BotCallbackButton Section(string title, string topicKey, string key) => new(
        title,
        CallbackStrings.SectionCallback(topicKey, key));

    public static BotLinkButton SectionLink(FrequentlyAskedSectionLink faqSectionLink) => new(
        faqSectionLink.Title,
        faqSectionLink.Url);

    public static BotCallbackButton EllipsisJustAnswer => new(
        "...",
        CallbackStrings.JustAnswerCallback);

    public static BotCallbackButton WriteCustomQuestion =>
        InternalAskCustomQuestion("Написать сообщение");

    public static BotCallbackButton AskCustomQuestion =>
        InternalAskCustomQuestion("Задать свой вопрос");

    public static BotCallbackButton TopicCustomQuestion =>
        InternalAskCustomQuestion("Нет нужной темы?");

    public static BotCallbackButton SectionCustomQuestion =>
        InternalAskCustomQuestion("Не нашли ответ?");

    public static BotCallbackButton ExpandCustomQuestion =>
        InternalAskCustomQuestion("Дополнить вопрос");

    public static BotCallbackButton ReplyAsCustomQuestion =>
        InternalAskCustomQuestion("Ответить");

    private static BotCallbackButton InternalAskCustomQuestion(string text) => new(
        text,
        CallbackStrings.AskCustomQuestionCallback);

    public static BotCallbackButton BlockUser(long targetUserId, BotMessengerType messengerType) => new(
        "Заблокировать",
        CallbackStrings.BlockUserCallback(targetUserId, messengerType));

    public static BotCallbackButton BackTo(string callback) => new(
        "Назад",
        callback);

    public static BotCallbackButton CancelTo(string callback) => new(
        "Отмена",
        callback);
}
