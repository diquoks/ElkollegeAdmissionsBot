using Max.Bot;
using Max.Bot.Configuration;
using Max.Bot.Types.Enums;
using Microsoft.Extensions.Options;

namespace ElkollegeAdmissionsBot.Services.Max;

public partial class MaxBotClient : IBotClient, IDisposable
{
    public MaxClient Implementation { get; }

    private readonly HttpClient _httpClient;
    private readonly HttpClient _pollingHttpClient;

    public MaxBotClient(IOptions<MaxBotOptions> options)
    {
        _httpClient = new HttpClient(UnvalidatedHttpClientHandler);
        _pollingHttpClient = new HttpClient(UnvalidatedHttpClientHandler);

        Implementation = new MaxClient(
            options: new global::Max.Bot.Configuration.MaxBotOptions
            {
                Token = options.Value.BotToken,
                BaseUrl = "https://platform-api2.max.ru",
                Polling = new MaxPollingOptions
                {
                    LongPollingTimeout = TimeSpan.FromSeconds(BotOptions.LongPollingTimeoutSeconds),
                    AllowedUpdateTypes =
                    [
                        UpdateType.MessageCreated,
                        UpdateType.MessageCallback,
                        UpdateType.MessageChatCreated
                    ]
                }
            },
            httpClient: _httpClient,
            pollingHttpClient: _pollingHttpClient);
    }

    // disables certificates validation
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
