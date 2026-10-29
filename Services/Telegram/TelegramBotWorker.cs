using Telegram.BotAPI.Extensions.LongPolling;

namespace ElkollegeAdmissionsBot.Services.Telegram;

public class TelegramBotWorker(ILogger<TelegramBotWorker> logger, TelegramBot bot, TelegramBotClient client)
    : BotWorker(logger, bot)
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await base.ExecuteAsync(stoppingToken);

        await client.Implementation.StartLongPolling(
            bot.OnUpdateAsync,
            bot.OnExceptionAsync,
            TelegramBotClient.LongPollingOptions,
            stoppingToken);
    }
}
