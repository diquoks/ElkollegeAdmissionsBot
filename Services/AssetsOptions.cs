namespace ElkollegeAdmissionsBot.Services;

public record AssetsOptions
{
    public const string SectionKey = "Assets";

    public required string FrequentlyAskedQuestionsPath { get; init; }
}
