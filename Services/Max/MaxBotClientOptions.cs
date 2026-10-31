namespace ElkollegeAdmissionsBot.Services.Max;

public record MaxBotClientOptions : BotClientOptions
{
    public const string Section = "Max";

    public required long AdminGroupId { get; init; }
}
