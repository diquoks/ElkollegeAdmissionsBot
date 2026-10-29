using ElkollegeAdmissionsBot.Models.Bot;
using Max.Bot.Polling;

namespace ElkollegeAdmissionsBot.Services.Max;

public class MaxBot(ILogger<MaxBot> logger, MaxBotClient client) : UpdateHandlerBase, IBot
{
    public BotHandler Handler { get; } = new(logger, client);

    public override async Task HandleMessageAsync(UpdateContext context, CancellationToken cancellationToken)
    {
        var message = context.Update.Message;

        if (message?.Text is null)
            return;

        if (message.Text.StartsWith('/'))
        {
            await HandleCommandAsync(context, cancellationToken);
            return;
        }

        await Handler.OnMessageAsync(BotMessage.FromMax(message), cancellationToken);
    }

    private Task HandleCommandAsync(UpdateContext context, CancellationToken cancellationToken) =>
        Handler.OnCommandAsync(BotMessage.FromMax(context.Update.Message!), cancellationToken);
}
