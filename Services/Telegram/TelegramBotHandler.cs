namespace ElkollegeAdmissionsBot.Services.Telegram;

public class TelegramBotHandler(
    ILogger<TelegramBotHandler> logger,
    DatabaseManager databaseManager,
    TelegramBotClient client,
    InterconnectionService interconnectionService)
    : BotHandler(logger, databaseManager, client, interconnectionService);
