using Telegram.BotAPI.Extensions.LongPolling;

namespace ElkollegeAdmissionsBot.Services.Telegram;

public class TelegramBotWorker(ILogger<TelegramBotWorker> logger, TelegramBot bot, TelegramBotClient client)
    : BotWorker(logger, bot)
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await base.ExecuteAsync(stoppingToken);

        await client.Implementation.StartLongPolling(
            updateHandler: bot.OnUpdateAsync,
            errorHandler: bot.OnExceptionAsync,
            options: TelegramBotClient.LongPollingOptions,
            cancellationToken: stoppingToken);
    }
}
