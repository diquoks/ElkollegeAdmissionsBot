using Telegram.BotAPI;
using Telegram.BotAPI.Extensions;
using Telegram.BotAPI.GettingUpdates;

namespace ElkollegeAdmissionsBot.Services.Telegram;

public class TelegramBot(ILogger<TelegramBot> logger, TelegramBotHandler handler)
    : SimpleUpdateHandlerBase, IBot<TelegramBotHandler>
{
    public TelegramBotHandler Handler { get; } = handler;

    public async Task PrepareAsync(CancellationToken cancellationToken = default)
    {
        var botUser = await Handler.PrepareAsync(cancellationToken);

        if (botUser.Username is not null)
            SetBotUserName(botUser.Username);
    }

    public Task OnUpdateAsync(ITelegramBotClient _, Update update, CancellationToken cancellationToken = default)
    {
        return OnUpdateAsync(update, cancellationToken);
    }

    public Task OnExceptionAsync(ITelegramBotClient _, Exception exp, CancellationToken cancellationToken = default)
    {
        return OnExceptionAsync(exp, cancellationToken);
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
