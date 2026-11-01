namespace ElkollegeAdmissionsBot.Services.Max;

public class MaxBotHandler(
    ILogger<MaxBotHandler> logger,
    DatabaseManager databaseManager,
    MaxBotClient client,
    InterconnectionService interconnectionService)
    : BotHandler(logger, databaseManager, client, interconnectionService);
