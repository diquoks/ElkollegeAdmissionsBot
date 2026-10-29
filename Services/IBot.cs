namespace ElkollegeAdmissionsBot.Services;

public interface IBot
{
    public BotHandler Handler { get; }

    public Task PrepareAsync(CancellationToken cancellationToken = default) =>
        Handler.PrepareAsync(cancellationToken);
}
