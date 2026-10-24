using ElkollegeAdmissionsBot.Models.Bot;
using Microsoft.Extensions.Options;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.AvailableTypes;
using Telegram.BotAPI.Extensions.LongPolling;
using Telegram.BotAPI.GettingUpdates;
using BotCommand = ElkollegeAdmissionsBot.Models.Bot.BotCommand;

namespace ElkollegeAdmissionsBot.Services.Telegram;

public class TelegramHandler(ILogger<TelegramHandler> logger, IOptions<TelegramHandlerOptions> options)
    : Handler(logger)
{
    public const string Section = "Telegram";

    public static LongPollingOptions LongPollingOptions { get; } = new()
    {
        AllowedUpdates = [UpdateTypes.Message, UpdateTypes.CallbackQuery],
        Timeout = LongPollingTimeoutSeconds
    };

    public TelegramBotClient Client { get; } = new(options.Value.BotToken);

    protected override async Task<BotUser> GetMeAsync(CancellationToken cancellationToken = default)
    {
        var botUser = await Client.GetMeAsync(cancellationToken);

        return BotUser.FromTelegram(botUser);
    }

    protected override async Task SetMyCommandsAsync(
        BotCommand[] commands,
        CancellationToken cancellationToken = default)
    {
        await Client.SetMyCommandsAsync(
            commands.Select(command => command.ToTelegram()),
            new BotCommandScopeAllPrivateChats(),
            cancellationToken: cancellationToken);
    }
}
