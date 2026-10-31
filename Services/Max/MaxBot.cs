using ElkollegeAdmissionsBot.Helpers;
using ElkollegeAdmissionsBot.Models.Bot;
using Max.Bot.Polling;
using Max.Bot.Types.Enums;
using Telegram.BotAPI.Extensions.Commands;

namespace ElkollegeAdmissionsBot.Services.Max;

public class MaxBot(ILogger<MaxBot> logger, DatabaseManager databaseManager, MaxBotClient client)
    : UpdateHandlerBase, IBot
{
    private BotUser? _botUser;

    public BotHandler Handler { get; } = new(logger, databaseManager, client);

    public async Task PrepareAsync(CancellationToken cancellationToken = default) =>
        _botUser = await Handler.PrepareAsync(cancellationToken);

    public override Task HandleMessageAsync(UpdateContext context, CancellationToken cancellationToken)
    {
        if (context.Update.Message is not { } message ||
            string.IsNullOrWhiteSpace(message.Mid) ||
            message.Recipient.ChatId is null ||
            message.Sender is null ||
            string.IsNullOrWhiteSpace(message.Text))
            return Task.CompletedTask;

        if (BotCommandParser.TryParse(message.Text, out var result))
        {
            if (message.Recipient.ChatType is ChatType.Dialog)
                return Handler.OnCommandAsync(BotMessage.FromMax(message), result.commandName, cancellationToken);

            if (string.IsNullOrWhiteSpace(result.username) || result.username != _botUser?.Username)
                return Task.CompletedTask;

            return Handler.OnCommandAsync(BotMessage.FromMax(message), result.commandName, cancellationToken);
        }

        if (message.Recipient.ChatType is ChatType.Dialog)
            return Handler.OnMessageAsync(BotMessage.FromMax(message), cancellationToken);

        if (message.Link?.Type is not MessageLinkType.Reply ||
            message.Link.Sender?.Id is not { } senderId ||
            senderId != _botUser?.Id)
            return Task.CompletedTask;

        return Handler.OnMessageAsync(BotMessage.FromMax(message), cancellationToken);
    }

    public override Task HandleCallbackQueryAsync(UpdateContext context, CancellationToken cancellationToken)
    {
        if (context.Update.Callback is not { } callbackQuery ||
            string.IsNullOrWhiteSpace(callbackQuery.Payload) ||
            context.Update.Message is not { } message ||
            string.IsNullOrWhiteSpace(message.Mid) ||
            message.Recipient.ChatId is null)
            return Task.CompletedTask;

        return Handler.OnCallbackQueryAsync(BotCallbackQuery.FromMax(callbackQuery, message), cancellationToken);
    }

    public override Task HandleUpdateAsync(UpdateContext context, CancellationToken cancellationToken)
    {
        switch (context.Update.Type)
        {
            case UpdateType.BotStarted:
                return client.SendMessageAsync(
                    chatId: context.Update.ChatId!.Value,
                    menu: BotMenus.Test(_botUser?.FullName ?? string.Empty),
                    cancellationToken: cancellationToken);
            default:
                return Task.CompletedTask;
        }
    }
}
