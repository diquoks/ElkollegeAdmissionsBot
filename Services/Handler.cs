using ElkollegeAdmissionsBot.Helpers;
using ElkollegeAdmissionsBot.Models.Bot;

namespace ElkollegeAdmissionsBot.Services;

public abstract class Handler(ILogger<Handler> logger)
{
    protected const short LongPollingTimeoutSeconds = 30;

    public async Task<BotUser> PrepareAsync(CancellationToken cancellationToken = default)
    {
        var botUser = await GetMeAsync(cancellationToken);

        await SetMyCommandsAsync(BotCommands.AllCommands, cancellationToken);

        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("Handler runs on @{BotUsername}.", botUser.Username);

        return botUser;
    }

    protected abstract Task<BotUser> GetMeAsync(CancellationToken cancellationToken = default);

    protected abstract Task SetMyCommandsAsync(BotCommand[] commands, CancellationToken cancellationToken = default);
}
