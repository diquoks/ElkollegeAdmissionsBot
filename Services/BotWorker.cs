namespace ElkollegeAdmissionsBot.Services;

public abstract class BotWorker(ILogger logger, IBot bot) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Worker started.");

        await bot.PrepareAsync(stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Worker is stopping...");

        await base.StopAsync(cancellationToken);
    }
}
