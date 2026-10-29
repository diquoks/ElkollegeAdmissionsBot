using ElkollegeAdmissionsBot.Models.Bot;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableTypes;
using Telegram.BotAPI.Extensions;
using Telegram.BotAPI.Extensions.Commands;
using Telegram.BotAPI.GettingUpdates;

namespace ElkollegeAdmissionsBot.Services.Telegram;

public class TelegramBot(ILogger<TelegramBot> logger, TelegramBotClient client) : SimpleUpdateHandlerBase, IBot
{
    public BotHandler Handler { get; } = new(logger, client);

    public async Task PrepareAsync(CancellationToken cancellationToken = default)
    {
        var botUser = await Handler.PrepareAsync(cancellationToken);

        if (!string.IsNullOrWhiteSpace(botUser.Username))
            SetBotUserName(botUser.Username);
    }

    public Task OnUpdateAsync(ITelegramBotClient _, Update update, CancellationToken cancellationToken = default) =>
        OnUpdateAsync(update, cancellationToken);

    public Task OnExceptionAsync(ITelegramBotClient _, Exception exp, CancellationToken cancellationToken = default) =>
        OnExceptionAsync(exp, cancellationToken);

    protected override async Task OnMessageAsync(Message message, CancellationToken cancellationToken = default)
    {
        if (message.From?.Id == TelegramConstants.TelegramId)
            return;

        if (BotCommandParser.TryParse(message, out _))
        {
            await base.OnMessageAsync(message, cancellationToken);
            return;
        }

        await Handler.OnMessageAsync(BotMessage.FromTelegram(message), cancellationToken);

        await base.OnMessageAsync(message, cancellationToken);
    }

    protected override Task OnCommandAsync(
        Message message,
        string commandName,
        string args,
        CancellationToken cancellationToken = default) =>
        Handler.OnCommandAsync(BotMessage.FromTelegram(message), cancellationToken);

    protected override Task OnExceptionAsync(Exception exp, CancellationToken cancellationToken = default)
    {
        switch (exp)
        {
            case OperationCanceledException:
            {
                if (cancellationToken.IsCancellationRequested)
                    return Task.FromCanceled(cancellationToken);

                logger.LogWarning(exp, "Request timed out.");
                break;
            }
            default:
            {
                logger.LogError(exp, "An exception occurred.");
                break;
            }
        }

        return Task.CompletedTask;
    }
}
