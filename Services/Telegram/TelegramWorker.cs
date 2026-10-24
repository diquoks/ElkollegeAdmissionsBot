using Telegram.BotAPI.Extensions.LongPolling;

namespace ElkollegeAdmissionsBot.Services.Telegram;

public class TelegramWorker(ILogger<TelegramWorker> logger, TelegramBot bot)
    : BotWorker<TelegramBot, TelegramHandler>(logger, bot)
{
    private readonly TelegramBot _bot = bot;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await base.ExecuteAsync(stoppingToken);

        await _bot.Handler.Client.StartLongPolling(
            _bot.OnUpdateAsync,
            _bot.OnExceptionAsync,
            TelegramHandler.LongPollingOptions,
            stoppingToken);
    }
}
