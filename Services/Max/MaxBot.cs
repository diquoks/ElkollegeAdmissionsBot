using ElkollegeAdmissionsBot.Extensions;
using ElkollegeAdmissionsBot.Helpers;
using ElkollegeAdmissionsBot.Models.Bot;
using Max.Bot.Polling;
using Max.Bot.Types.Enums;
using Telegram.BotAPI.Extensions.Commands;

namespace ElkollegeAdmissionsBot.Services.Max;

public class MaxBot(
    ILogger<MaxBot> logger,
    FaqProvider faqProvider,
    DatabaseManager databaseManager,
    MaxBotClient client,
    InterconnectionService interconnectionService) : UpdateHandlerBase, IBot
{
    private readonly BotHandler _handler = new(logger, faqProvider, databaseManager, client, interconnectionService);

    private BotUser _botUser = null!;

    public async Task PrepareAsync(CancellationToken cancellationToken = default) =>
        _botUser = await _handler.PrepareAsync(cancellationToken);

    public override async Task HandleMessageAsync(UpdateContext context, CancellationToken cancellationToken)
    {
        if (context.Update.Message is not { } message ||
            string.IsNullOrWhiteSpace(message.Mid) ||
            message.Recipient.ChatId is null ||
            message.Sender is null ||
            string.IsNullOrWhiteSpace(message.Text))
            return;

        if (BotCommandParser.TryParse(message.Text, out var result))
        {
            if (message.Recipient.ChatType is ChatType.Dialog)
            {
                await _handler.OnCommandAsync(BotMessage.FromMax(message), result.commandName, cancellationToken);
                return;
            }

            if (string.IsNullOrWhiteSpace(result.username) || result.username != _botUser.Username)
                return;

            await _handler.OnCommandAsync(BotMessage.FromMax(message), result.commandName, cancellationToken);
            return;
        }

        if (message.Recipient.ChatType is ChatType.Dialog)
        {
            await _handler.OnMessageAsync(BotMessage.FromMax(message), cancellationToken);
            return;
        }

        if (message.Link?.Type is not MessageLinkType.Reply ||
            message.Link.Sender?.Id is not { } senderId ||
            senderId != _botUser.Id)
            return;

        await _handler.OnMessageAsync(BotMessage.FromMax(message), cancellationToken);
    }

    public override async Task HandleCallbackQueryAsync(UpdateContext context, CancellationToken cancellationToken)
    {
        if (context.Update.Callback is not { } callbackQuery ||
            string.IsNullOrWhiteSpace(callbackQuery.Payload) ||
            context.Update.Message is not { } message ||
            string.IsNullOrWhiteSpace(message.Mid) ||
            message.Recipient.ChatId is null)
            return;

        await _handler.OnCallbackQueryAsync(BotCallbackQuery.FromMax(callbackQuery, message), cancellationToken);
    }

    public override async Task HandleUpdateAsync(UpdateContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(context.Update.UpdateTypeRaw))
            return;

        var chatId = context.Update.ChatId!.Value;

        logger.LogBotInteraction(
            user: BotUser.FromMax(context.Update.User!),
            interaction: context.Update.UpdateTypeRaw);

        switch (context.Update.Type)
        {
            case UpdateType.BotStarted:
            {
                await client.SendMessageAsync(
                    chatId: chatId,
                    menu: BotMenus.Start(
                        botName: _botUser.FullName,
                        addCustomQuestionButton: !interconnectionService.IsInAdminGroup(BotMessengerType.Max, chatId),
                        faqTopics: faqProvider.Faq.Topics),
                    cancellationToken: cancellationToken);
                break;
            }
        }
    }
}
