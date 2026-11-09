namespace ElkollegeAdmissionsBot.Helpers;

public static class CallbackStrings
{
    public const string Start =
        "start";

    public const string Topic =
        "topic";

    public const string Section =
        "section";

    public const string AskCustomQuestion =
        "ask_custom_question";

    public const string JustAnswer =
        "just_answer";

    public static string StartCallback =>
        Start;

    public static string TopicCallback(string key) =>
        $"{Topic} {key}";

    public static string SectionCallback(string key, string topicKey) =>
        $"{Section} {topicKey} {key}";

    public static string AskCustomQuestionCallback =>
        AskCustomQuestion;

    public static string JustAnswerCallback =>
        JustAnswer;
}
