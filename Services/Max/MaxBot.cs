using ElkollegeAdmissionsBot.Extensions;
using ElkollegeAdmissionsBot.Helpers;
using ElkollegeAdmissionsBot.Models.Bot;
using Max.Bot.Polling;
using Max.Bot.Types.Enums;
using Telegram.BotAPI.Extensions.Commands;

namespace ElkollegeAdmissionsBot.Services.Max;

public class MaxBot(
    ILogger<MaxBot> logger,
    DatabaseManager databaseManager,
    MaxBotClient client,
    InterconnectionService interconnectionService) : UpdateHandlerBase, IBot
{
    private readonly BotHandler _handler = new(logger, databaseManager, client, interconnectionService);

    private BotUser _botUser = null!;

    public async Task PrepareAsync(CancellationToken cancellationToken = default) =>
        _botUser = await _handler.PrepareAsync(cancellationToken);

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
                return _handler.OnCommandAsync(BotMessage.FromMax(message), result.commandName, cancellationToken);

            if (string.IsNullOrWhiteSpace(result.username) || result.username != _botUser.Username)
                return Task.CompletedTask;

            return _handler.OnCommandAsync(BotMessage.FromMax(message), result.commandName, cancellationToken);
        }

        if (message.Recipient.ChatType is ChatType.Dialog)
            return _handler.OnMessageAsync(BotMessage.FromMax(message), cancellationToken);

        if (message.Link?.Type is not MessageLinkType.Reply ||
            message.Link.Sender?.Id is not { } senderId ||
            senderId != _botUser.Id)
            return Task.CompletedTask;

        return _handler.OnMessageAsync(BotMessage.FromMax(message), cancellationToken);
    }

    public override Task HandleCallbackQueryAsync(UpdateContext context, CancellationToken cancellationToken)
    {
        if (context.Update.Callback is not { } callbackQuery ||
            string.IsNullOrWhiteSpace(callbackQuery.Payload) ||
            context.Update.Message is not { } message ||
            string.IsNullOrWhiteSpace(message.Mid) ||
            message.Recipient.ChatId is null)
            return Task.CompletedTask;

        return _handler.OnCallbackQueryAsync(BotCallbackQuery.FromMax(callbackQuery, message), cancellationToken);
    }

    public override Task HandleUpdateAsync(UpdateContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(context.Update.UpdateTypeRaw))
            return Task.CompletedTask;

        var chatId = context.Update.ChatId!.Value;

        logger.LogBotInteraction(
            user: BotUser.FromMax(context.Update.User!),
            interaction: context.Update.UpdateTypeRaw);

        switch (context.Update.Type)
        {
            case UpdateType.BotStarted:
                return client.SendMessageAsync(
                    chatId: chatId,
                    menu: BotMenus.Start(
                        botName: _botUser.FullName,
                        addCustomQuestionButton: !interconnectionService.IsInAdminGroup(BotMessengerType.Max, chatId)),
                    cancellationToken: cancellationToken);
            default:
                return Task.CompletedTask;
        }
    }
}
