namespace ElkollegeAdmissionsBot.Services.Telegram;

public class TelegramBotHandler(ILogger<TelegramBotHandler> logger, TelegramBotClient client)
    : BotHandler(logger, client);
