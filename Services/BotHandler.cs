using ElkollegeAdmissionsBot.Helpers;
using ElkollegeAdmissionsBot.Models.Bot;

namespace ElkollegeAdmissionsBot.Services;

public abstract class BotHandler(ILogger logger, IBotClient client)
{
    public async Task<BotUser> PrepareAsync(CancellationToken cancellationToken = default)
    {
        var botUser = await client.GetMeAsync(cancellationToken);

        await client.SetMyCommandsAsync(BotCommands.AllCommands, cancellationToken);

        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("Handler runs on @{BotUsername}.", botUser.Username);

        return botUser;
    }
}
