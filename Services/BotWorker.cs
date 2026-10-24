namespace ElkollegeAdmissionsBot.Services;

public abstract class BotWorker<TBot, THandler>(ILogger<BotWorker<TBot, THandler>> logger, TBot bot)
    : BackgroundService where TBot : IBot<THandler> where THandler : Handler
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("Worker started.");

        return bot.PrepareAsync(stoppingToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("Worker is stopping...");

        return base.StopAsync(cancellationToken);
    }
}
