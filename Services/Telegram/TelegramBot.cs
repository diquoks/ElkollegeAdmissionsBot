using ElkollegeAdmissionsBot.Models.Bot;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableTypes;
using Telegram.BotAPI.Extensions;
using Telegram.BotAPI.Extensions.Commands;
using Telegram.BotAPI.GettingUpdates;

namespace ElkollegeAdmissionsBot.Services.Telegram;

public class TelegramBot(
    ILogger<TelegramBot> logger,
    DatabaseManager databaseManager,
    TelegramBotClient client,
    InterconnectionService interconnectionService) : SimpleUpdateHandlerBase, IBot
{
    private readonly BotHandler _handler = new(logger, databaseManager, client, interconnectionService);

    public async Task PrepareAsync(CancellationToken cancellationToken = default)
    {
        var botUser = await _handler.PrepareAsync(cancellationToken);

        if (!string.IsNullOrWhiteSpace(botUser.Username))
            SetBotUserName(botUser.Username);
    }

    public Task OnUpdateAsync(ITelegramBotClient _, Update update, CancellationToken cancellationToken = default) =>
        OnUpdateAsync(update, cancellationToken);

    public Task OnExceptionAsync(ITelegramBotClient _, Exception exp, CancellationToken cancellationToken = default) =>
        OnExceptionAsync(exp, cancellationToken);

    protected override async Task OnMessageAsync(Message message, CancellationToken cancellationToken = default)
    {
        if (message.From?.Id is null or TelegramConstants.TelegramId || string.IsNullOrWhiteSpace(message.Text))
            return;

        if (BotCommandParser.TryParse(message, out _))
        {
            await base.OnMessageAsync(message, cancellationToken);
            return;
        }

        await _handler.OnMessageAsync(BotMessage.FromTelegram(message), cancellationToken);

        await base.OnMessageAsync(message, cancellationToken);
    }

    protected override Task OnCommandAsync(Message message, string commandName, string args,
        CancellationToken cancellationToken = default) =>
        _handler.OnCommandAsync(BotMessage.FromTelegram(message), commandName, cancellationToken);

    protected override Task OnCallbackQueryAsync(CallbackQuery callbackQuery,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(callbackQuery.Data) || callbackQuery.Message is null)
            return Task.CompletedTask;

        return _handler.OnCallbackQueryAsync(BotCallbackQuery.FromTelegram(callbackQuery), cancellationToken);
    }

    protected override Task OnExceptionAsync(Exception exp, CancellationToken cancellationToken = default)
    {
        switch (exp)
        {
            case OperationCanceledException:
            {
                if (cancellationToken.IsCancellationRequested)
                    return Task.FromCanceled(cancellationToken);

                logger.LogWarning(exp, "Request timed out.");

                return Task.CompletedTask;
            }
            default:
            {
                logger.LogError(exp, "An exception occurred.");

                return Task.CompletedTask;
            }
        }
    }
}
