using Max.Bot;
using Max.Bot.Configuration;
using Max.Bot.Types.Enums;
using Microsoft.Extensions.Options;

namespace ElkollegeAdmissionsBot.Services.Max;

public partial class MaxBotClient : IBotClient, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly HttpClient _pollingHttpClient;

    public MaxClient Implementation { get; }

    public long AdminGroupId { get; }

    public MaxBotClient(IOptions<MaxBotClientOptions> options, ILoggerFactory loggerFactory)
    {
        _httpClient = new HttpClient(UnvalidatedHttpClientHandler);
        _pollingHttpClient = new HttpClient(UnvalidatedHttpClientHandler);

        Implementation = new MaxClient(
            options: new MaxBotOptions
            {
                Token = options.Value.BotToken,
                BaseUrl = "https://platform-api2.max.ru",
                Polling = new MaxPollingOptions
                {
                    LongPollingTimeout = TimeSpan.FromSeconds(BotClientOptions.LongPollingTimeoutSeconds),
                    AllowedUpdateTypes =
                    [
                        UpdateType.MessageCreated,
                        UpdateType.MessageCallback,
                        UpdateType.BotStarted,
                        UpdateType.MessageChatCreated
                    ]
                }
            },
            httpClient: _httpClient,
            pollingHttpClient: _pollingHttpClient,
            loggerFactory: loggerFactory);

        AdminGroupId = options.Value.AdminGroupId;
    }

    // TODO: disables certificates validation for cases where trusted certificates for requests to MAX aren't installed.
    //       maybe it would be worth providing a way to include such certificates into the Docker image for better
    //       security, while keeping the current approach for local development and testing only
    private static HttpClientHandler UnvalidatedHttpClientHandler => new()
    {
        ServerCertificateCustomValidationCallback = (_, _, _, _) => true
    };

    public void Dispose()
    {
        Implementation.Dispose();

        _httpClient.Dispose();
        _pollingHttpClient.Dispose();

        GC.SuppressFinalize(this);
    }
}
