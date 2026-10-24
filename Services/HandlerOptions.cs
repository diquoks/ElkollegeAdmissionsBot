namespace ElkollegeAdmissionsBot.Services;

public abstract record HandlerOptions
{
    public required string BotToken
    {
        get
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(field, nameof(BotToken));

            return field;
        }
        init;
    }
}
