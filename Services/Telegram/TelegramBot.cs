using ElkollegeAdmissionsBot.Models.Bot;
using Telegram.BotAPI;
using Telegram.BotAPI.AvailableTypes;
using Telegram.BotAPI.Extensions;
using Telegram.BotAPI.Extensions.Commands;
using Telegram.BotAPI.GettingUpdates;

namespace ElkollegeAdmissionsBot.Services.Telegram;

public class TelegramBot(
    ILogger<TelegramBot> logger,
    FaqProvider faqProvider,
    DatabaseManager databaseManager,
    TelegramBotClient client,
    InterconnectionService interconnectionService) : SimpleUpdateHandlerBase, IBot
{
    private readonly BotHandler _handler = new(logger, faqProvider, databaseManager, client, interconnectionService);

    public async Task PrepareAsync(CancellationToken cancellationToken = default)
    {
        var botUser = await _handler.PrepareAsync(cancellationToken);

        if (!string.IsNullOrWhiteSpace(botUser.Username))
            SetBotUserName(botUser.Username);
    }

    public async Task OnUpdateAsync(ITelegramBotClient _, Update update,
        CancellationToken cancellationToken = default) =>
        await OnUpdateAsync(update, cancellationToken);

    public async Task OnExceptionAsync(ITelegramBotClient _, Exception exp,
        CancellationToken cancellationToken = default) =>
        await OnExceptionAsync(exp, cancellationToken);

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

    protected override async Task OnCommandAsync(Message message, string commandName, string args,
        CancellationToken cancellationToken = default) =>
        await _handler.OnCommandAsync(BotMessage.FromTelegram(message), commandName, cancellationToken);

    protected override async Task OnCallbackQueryAsync(CallbackQuery callbackQuery,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(callbackQuery.Data) || callbackQuery.Message is null)
            return;

        await _handler.OnCallbackQueryAsync(BotCallbackQuery.FromTelegram(callbackQuery), cancellationToken);
    }

    // ReSharper disable once AsyncMethodWithoutAwait
    protected override async Task OnExceptionAsync(Exception exp, CancellationToken cancellationToken = default)
    {
        switch (exp)
        {
            case OperationCanceledException:
            {
                cancellationToken.ThrowIfCancellationRequested();

                logger.LogWarning(exp, "Request timed out.");

                break;
            }
            default:
            {
                logger.LogError(exp, "An exception occurred.");

                break;
            }
        }
    }
}
