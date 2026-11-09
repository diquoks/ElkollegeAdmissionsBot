namespace ElkollegeAdmissionsBot.Services.Max;

public record MaxBotClientOptions : BotClientOptions
{
    public const string SectionKey = "Max";

    public required long AdminGroupId { get; init; }
}
