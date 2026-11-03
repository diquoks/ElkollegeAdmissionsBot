using ElkollegeAdmissionsBot.Helpers;
using ElkollegeAdmissionsBot.Models.Bot;

namespace ElkollegeAdmissionsBot.Services;

public partial class BotHandler(
    ILogger logger,
    DatabaseManager databaseManager,
    IBotClient client,
    InterconnectionService interconnectionService)
{
    private BotUser _botUser = null!;

    public async Task<BotUser> PrepareAsync(CancellationToken cancellationToken = default)
    {
        _botUser = await client.GetMeAsync(cancellationToken);

        await client.SetMyCommandsAsync(
            commands: BotCommands.AllCommands,
            cancellationToken: cancellationToken);

        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("Handler runs on @{BotUsername}.", _botUser.Username);

        return _botUser;
    }
}
