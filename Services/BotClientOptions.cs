namespace ElkollegeAdmissionsBot.Services;

public abstract record BotClientOptions
{
    public const int LongPollingTimeoutSeconds = 30;

    public required string BotToken { get; init; }
}
