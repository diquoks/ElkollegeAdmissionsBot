using ElkollegeAdmissionsBot.Models.Bot;
using Max.Bot;
using Max.Bot.Configuration;
using Max.Bot.Types.Enums;
using Microsoft.Extensions.Options;

namespace ElkollegeAdmissionsBot.Services.Max;

public class MaxHandler(ILogger<MaxHandler> logger, IOptions<MaxHandlerOptions> options) : Handler(logger), IDisposable
{
    public const string Section = "Max";

    public MaxClient Client { get; } = new(
        new MaxBotOptions
        {
            Token = options.Value.BotToken,
            BaseUrl = "https://platform-api2.max.ru",
            Polling = new MaxPollingOptions
            {
                LongPollingTimeout = TimeSpan.FromSeconds(LongPollingTimeoutSeconds),
                AllowedUpdateTypes = [UpdateType.MessageCreated, UpdateType.MessageCallback]
            }
        },
        new HttpClient(new HttpClientHandler { ServerCertificateCustomValidationCallback = (_, _, _, _) => true }));

    protected override async Task<BotUser> GetMeAsync(CancellationToken cancellationToken = default)
    {
        var botUser = await Client.Bot.GetMeAsync(cancellationToken);

        return BotUser.FromMax(botUser);
    }

    protected override Task SetMyCommandsAsync(
        BotCommand[] commands,
        CancellationToken cancellationToken = default)
    {
        // TODO: https://github.com/NanoAgents/MaxBotNet/issues/41
        //  await Client.Bot.SetCommandsAsync(commands.Select(command => command.ToMax()), cancellationToken);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        Client.Dispose();

        GC.SuppressFinalize(this);
    }
}
