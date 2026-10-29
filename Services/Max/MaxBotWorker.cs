namespace ElkollegeAdmissionsBot.Services.Max;

public class MaxBotWorker(ILogger<MaxBotWorker> logger, MaxBot bot, MaxBotClient client) : BotWorker(logger, bot)
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await base.ExecuteAsync(stoppingToken);

        await client.Implementation.StartPollingAsync(bot, cancellationToken: stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await client.Implementation.StopPollingAsync(cancellationToken);

        await base.StopAsync(cancellationToken);
    }
}
