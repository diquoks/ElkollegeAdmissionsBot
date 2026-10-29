namespace ElkollegeAdmissionsBot.Services;

public abstract class BotWorker(ILogger logger, IBot bot) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Worker started.");

        return bot.PrepareAsync(stoppingToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Worker is stopping...");

        return base.StopAsync(cancellationToken);
    }
}
