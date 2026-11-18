using ElkollegeAdmissionsBot.Models.Bot;

namespace ElkollegeAdmissionsBot.Helpers;

public static class CallbackStrings
{
    public const char CallbackSeparator = ' ';

    public const string Start =
        "start";

    public const string Topic =
        "topic";

    public const string Section =
        "section";

    public const string AskCustomQuestion =
        "ask_custom_question";

    public const string BlockUser =
        "block_user";

    public const string JustAnswer =
        "just_answer";

    public static string StartCallback =>
        BuildCallback(Start);

    public static string TopicCallback(string key) =>
        BuildCallback(Topic, key);

    public static string SectionCallback(string topicKey, string key) =>
        BuildCallback(Section, topicKey, key);

    public static string AskCustomQuestionCallback =>
        BuildCallback(AskCustomQuestion);

    public static string BlockUserCallback(long targetUserId, BotMessengerType messengerType) =>
        BuildCallback(BlockUser, targetUserId, (int)messengerType);

    public static string JustAnswerCallback =>
        BuildCallback(JustAnswer);

    private static string BuildCallback(string callback, params object[] args) =>
        string.Join(CallbackSeparator, [callback, .. args]);
}
