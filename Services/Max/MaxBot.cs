using System.Text.Json;
using ElkollegeAdmissionsBot.Models.Bot;
using Max.Bot.Polling;
using Max.Bot.Types.Enums;
using Telegram.BotAPI.Extensions.Commands;

namespace ElkollegeAdmissionsBot.Services.Max;

public class MaxBot(ILogger<MaxBot> logger, MaxBotClient client) : UpdateHandlerBase, IBot
{
    private string? _botUsername;

    public BotHandler Handler { get; } = new(logger, client);

    public async Task PrepareAsync(CancellationToken cancellationToken = default)
    {
        var botUser = await Handler.PrepareAsync(cancellationToken);

        if (!string.IsNullOrWhiteSpace(botUser.Username))
            _botUsername = botUser.Username;
    }

    public override Task HandleMessageAsync(UpdateContext context, CancellationToken cancellationToken)
    {
        var message = context.Update.Message;

        if (message?.Text is null)
            return Task.CompletedTask;

        if (!BotCommandParser.TryParse(message.Text, out var result))
            return Handler.OnMessageAsync(BotMessage.FromMax(message), cancellationToken);

        var chatType = JsonSerializer.SerializeToElement(message.Recipient!.ChatType!).Deserialize<ChatType>();

        if (chatType is not ChatType.Dialog &&
            (string.IsNullOrWhiteSpace(result.username) || result.username != _botUsername))
            return Task.CompletedTask;

        return Handler.OnCommandAsync(BotMessage.FromMax(message), result.commandName, cancellationToken);
    }
}
