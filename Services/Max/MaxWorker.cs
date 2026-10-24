namespace ElkollegeAdmissionsBot.Services.Max;

public class MaxWorker(ILogger<MaxWorker> logger, MaxBot bot) : BotWorker<MaxBot, MaxHandler>(logger, bot)
{
    private readonly MaxBot _bot = bot;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await base.ExecuteAsync(stoppingToken);

        await _bot.Handler.Client.StartPollingAsync(_bot, cancellationToken: stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await _bot.Handler.Client.StopPollingAsync(cancellationToken);

        await base.StopAsync(cancellationToken);
    }
}
