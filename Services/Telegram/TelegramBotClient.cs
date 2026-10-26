using ElkollegeAdmissionsBot.Models.Bot;
using Microsoft.Extensions.Options;
using Telegram.BotAPI.AvailableMethods;
using Telegram.BotAPI.AvailableTypes;
using Telegram.BotAPI.Extensions.LongPolling;
using Telegram.BotAPI.GettingUpdates;
using BotCommand = ElkollegeAdmissionsBot.Models.Bot.BotCommand;

namespace ElkollegeAdmissionsBot.Services.Telegram;

public class TelegramBotClient(IOptions<TelegramBotOptions> options) : IBotClient
{
    public static LongPollingOptions LongPollingOptions { get; } = new()
    {
        AllowedUpdates = [UpdateTypes.Message, UpdateTypes.CallbackQuery],
        Timeout = BotOptions.LongPollingTimeoutSeconds
    };

    public global::Telegram.BotAPI.TelegramBotClient Implementation { get; } = new(options.Value.BotToken);

    public async Task<BotUser> GetMeAsync(CancellationToken cancellationToken = default)
    {
        var botUser = await Implementation.GetMeAsync(cancellationToken);

        return BotUser.FromTelegram(botUser);
    }

    public async Task SetMyCommandsAsync(BotCommand[] commands, CancellationToken cancellationToken = default)
    {
        await Implementation.SetMyCommandsAsync(
            commands.Select(command => command.ToTelegram()),
            new BotCommandScopeAllPrivateChats(),
            cancellationToken: cancellationToken);
    }
}
