using Microsoft.Extensions.Options;
using Telegram.BotAPI.Extensions.LongPolling;
using Telegram.BotAPI.GettingUpdates;
using TelegramClient = Telegram.BotAPI.TelegramBotClient;

namespace ElkollegeAdmissionsBot.Services.Telegram;

public partial class TelegramBotClient(IOptions<TelegramBotClientOptions> options) : IBotClient
{
    public static LongPollingOptions LongPollingOptions { get; } = new()
    {
        AllowedUpdates =
        [
            UpdateTypes.Message,
            UpdateTypes.CallbackQuery
        ],
        Timeout = BotClientOptions.LongPollingTimeoutSeconds
    };

    public TelegramClient Implementation { get; } = new(options.Value.BotToken);
}
