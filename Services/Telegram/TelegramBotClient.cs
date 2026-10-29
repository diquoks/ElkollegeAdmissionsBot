using Microsoft.Extensions.Options;
using Telegram.BotAPI.Extensions.LongPolling;
using Telegram.BotAPI.GettingUpdates;

namespace ElkollegeAdmissionsBot.Services.Telegram;

public partial class TelegramBotClient(IOptions<TelegramBotOptions> options) : IBotClient
{
    public static LongPollingOptions LongPollingOptions { get; } = new()
    {
        AllowedUpdates = [UpdateTypes.Message, UpdateTypes.CallbackQuery],
        Timeout = BotOptions.LongPollingTimeoutSeconds
    };

    public global::Telegram.BotAPI.TelegramBotClient Implementation { get; } = new(options.Value.BotToken);
}
