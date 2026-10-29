using Max.Bot;
using Max.Bot.Configuration;
using Max.Bot.Types.Enums;
using Microsoft.Extensions.Options;

namespace ElkollegeAdmissionsBot.Services.Max;

public partial class MaxBotClient(IOptions<MaxBotOptions> options) : IBotClient, IDisposable
{
    public MaxClient Implementation { get; } = new(
        new global::Max.Bot.Configuration.MaxBotOptions
        {
            Token = options.Value.BotToken,
            // TODO: not working for some reason?
            //  BaseUrl = "https://platform-api2.max.ru",
            Polling = new MaxPollingOptions
            {
                LongPollingTimeout = TimeSpan.FromSeconds(BotOptions.LongPollingTimeoutSeconds),
                AllowedUpdateTypes = [UpdateType.MessageCreated, UpdateType.MessageCallback]
            }
        },
        // disable certificates validation for now
        new HttpClient(new HttpClientHandler { ServerCertificateCustomValidationCallback = (_, _, _, _) => true }));

    public void Dispose()
    {
        Implementation.Dispose();

        GC.SuppressFinalize(this);
    }
}
