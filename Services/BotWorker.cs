namespace ElkollegeAdmissionsBot.Services;

public abstract class BotWorker<THandler>(ILogger logger, IBot<THandler> bot)
    : BackgroundService where THandler : BotHandler
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
