using ElkollegeAdmissionsBot.Models.Bot;
using Max.Bot;
using Max.Bot.Configuration;
using Max.Bot.Types.Enums;
using Microsoft.Extensions.Options;

namespace ElkollegeAdmissionsBot.Services.Max;

public class MaxBotClient(IOptions<MaxBotOptions> options) : IBotClient, IDisposable
{
    public MaxClient Implementation { get; } = new(
        new global::Max.Bot.Configuration.MaxBotOptions
        {
            Token = options.Value.BotToken,
            BaseUrl = "https://platform-api2.max.ru",
            Polling = new MaxPollingOptions
            {
                LongPollingTimeout = TimeSpan.FromSeconds(BotOptions.LongPollingTimeoutSeconds),
                AllowedUpdateTypes = [UpdateType.MessageCreated, UpdateType.MessageCallback]
            }
        },
        // disable certificates validation
        new HttpClient(new HttpClientHandler { ServerCertificateCustomValidationCallback = (_, _, _, _) => true }));

    public async Task<BotUser> GetMeAsync(CancellationToken cancellationToken = default)
    {
        var botUser = await Implementation.Bot.GetMeAsync(cancellationToken);

        return BotUser.FromMax(botUser);
    }

    public Task SetMyCommandsAsync(BotCommand[] commands, CancellationToken cancellationToken = default)
    {
        // TODO: https://github.com/NanoAgents/MaxBotNet/issues/41
        //  await Implementation.Bot.SetCommandsAsync(commands.Select(command => command.ToMax()), cancellationToken);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        Implementation.Dispose();

        GC.SuppressFinalize(this);
    }
}
